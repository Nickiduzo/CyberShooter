using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaUI : NetworkBehaviour
{
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject tabInformation;

    [Header("Text's prefabs")]
    [SerializeField] private GameObject playerName;
    [SerializeField] private GameObject playerKills;
    [SerializeField] private GameObject playerDeathAmount;
    [SerializeField] private GameObject playerStatistic;

    [Header("Spaces")]
    [SerializeField] private Transform namesSpace;
    [SerializeField] private Transform deathsSpace;
    [SerializeField] private Transform killsSpace;
    [SerializeField] private Transform KDSpace;

    [SerializeField] private TextMeshProUGUI textDamage;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private string[] damageAnimationNames;

    private void Awake()
    {
        exitButton.onClick.AddListener(ExitFromArena);

        Sword.ShowDamage += ShowDamageHandler;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        gameObject.SetActive(IsOwner);
    }

    private void Update()
    {
        if (!IsOwner) return;

        if(Input.GetKeyDown(KeyCode.Escape) && !Cursor.visible)
        {
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible)
        {
            Cursor.visible = false;
        }

        if(Input.GetMouseButtonDown(0) && Cursor.visible)
        {
            Cursor.visible = false;
        }


        if(Input.GetKeyDown(KeyCode.Tab) && !tabInformation.activeSelf)
        {
            tabInformation.SetActive(true);
            FillPlayerList(PlayerSpawner.Instance.GetAllPlayers());
            print(PlayerSpawner.Instance.GetAllPlayers().Count);
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && tabInformation.activeSelf) 
        {
            tabInformation.SetActive(false);
        }
    }

    private void FillPlayerList(List<PlayerStats> players)
    {
        ClearAllContents();

        foreach (PlayerStats player in players)
        {
            GameObject nameEntry = Instantiate(playerName, namesSpace);
            TextMeshProUGUI nameText = nameEntry.GetComponent<TextMeshProUGUI>();
            nameText.text = player.name;

            GameObject killsEntry = Instantiate(playerKills, killsSpace);
            TextMeshProUGUI killText = killsEntry.GetComponent<TextMeshProUGUI>();
            killText.text = player.Kills.Value.ToString();

            GameObject deathEntry = Instantiate(playerDeathAmount, deathsSpace);
            TextMeshProUGUI deathText = deathEntry.GetComponent<TextMeshProUGUI>();
            deathText.text = player.Deaths.Value.ToString();

            GameObject statistic = Instantiate(playerStatistic, KDSpace);
            TextMeshProUGUI statisticText = statistic.GetComponent<TextMeshProUGUI>();
            statisticText.text = player.KD.ToString();
        }
    }


    private void ClearAllContents()
    {
        foreach (Transform child in namesSpace)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in deathsSpace)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in killsSpace)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in KDSpace)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowDamageHandler(int damage)
    {
        if (!IsOwner) return;
        textDamage.gameObject.SetActive(true);
    
        int randomIndex = Random.Range(0, damageAnimationNames.Length - 1);
        textDamage.text = damage.ToString();
        damageAnimator.Play(damageAnimationNames[randomIndex]);
    }

    private void ExitFromArena()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("Menu");
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        Sword.ShowDamage -= ShowDamageHandler;
    }
}
