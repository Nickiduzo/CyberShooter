using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaUI : NetworkBehaviour
{
    [SerializeField] private Button exitButton;

    [SerializeField] private GameObject tabInformation;

    [SerializeField] private TextMeshProUGUI textDamage;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private string[] damageAnimationNames;

    private void Awake()
    {
        exitButton.onClick.AddListener(ExitFromArena);

        Sword.ShowDamage += ShowDamageHandler;

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
        }
        else if(Input.GetKeyDown(KeyCode.Tab) && tabInformation.activeSelf) 
        {
            tabInformation.SetActive(false);
        }
    }

    private void ShowDamageHandler(int damage)
    {
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
