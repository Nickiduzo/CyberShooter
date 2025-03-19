using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [Header("Play Panel")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [Header("Buttons Character Hadnler")]
    [SerializeField] private Button colorButton;
    [SerializeField] private Button swordButton;
    [SerializeField] private Button fastSwordsButton;
    [SerializeField] private Button slowSwordsButton;
    [SerializeField] private Button backButton;

    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera swordCamera;
    [SerializeField] private Camera swordsCamera;
    [SerializeField] private Camera colorCamera;
    [SerializeField] private Camera fastSwordsCamera;
    [SerializeField] private Camera menuCamera;
    private float transitionSpeed = 1.5f;

    [Header("Character Panel")]
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject player;
    [SerializeField] private SkinnedMeshRenderer skin;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Button purple;
    [SerializeField] private Button yellow;
    [SerializeField] private Button green;
    [SerializeField] private Button red;
    [SerializeField] private TMP_InputField inputField;

    [Header("Sword Panel")]
    [SerializeField] private TextMeshProUGUI swordName;
    [SerializeField] private Button swordRightArrow;
    [SerializeField] private Button swordLeftArrow;
    [SerializeField] private GameObject[] hardSwords;
    private int currentSword = 0;

    [Header("Swords Panel")]
    [SerializeField] private TextMeshProUGUI swordsName;
    [SerializeField] private Button swordsRightArrowButton;
    [SerializeField] private Button swordsLeftArrowButton;
    [SerializeField] private GameObject[] swords;
    private int currentSwords = 0;

    [Header("Fast Swords Panel")]
    [SerializeField] private TextMeshProUGUI fastSwordsName;
    [SerializeField] private Button fastRightArrowButton;
    [SerializeField] private Button fastLeftArrowButton;
    [SerializeField] private GameObject[] fastSwords;
    private int currentFastSwords = 0;

    [SerializeField] private Button exitButton;

    private void Start()
    {
        Cursor.visible = true;

        InitializeCharacterButtons();
        InitializeData();

        AudioManager.Instance.PlayRandomTrack();
    }

    private void InitializeCharacterButtons()
    {
        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);
        exitButton.onClick.AddListener(ExitGame);

        colorButton.onClick.AddListener(ChooseColor);
        swordButton.onClick.AddListener(ChooseSword);
        fastSwordsButton.onClick.AddListener(ChooseFastSwords);
        slowSwordsButton.onClick.AddListener(ChooseUsualSwords);

        backButton.onClick.AddListener(ChooseMainMenu);

        fastRightArrowButton.onClick.AddListener(RightFastSword);
        fastLeftArrowButton.onClick.AddListener(LeftFastSword);

        swordRightArrow.onClick.AddListener(RightSword);
        swordLeftArrow.onClick.AddListener(LeftSword);

        swordsRightArrowButton.onClick.AddListener(RightSwords);
        swordsLeftArrowButton.onClick.AddListener(LeftSwords);

        red.onClick.AddListener(ChooseRed);
        purple.onClick.AddListener(ChoosePurple);
        green.onClick.AddListener(ChooseGreen);
        yellow.onClick.AddListener(ChooseYellow);

        slider.onValueChanged.AddListener(RotatePlayer);
        inputField.onValueChanged.AddListener(ValidateAndDisplayName);
    }

    private void InitializeData()
    {
        playerName.text = playerData.currentName;

        switch (playerData.currentColor)
        {
            case 0: ChoosePurple();
                break;
            case 1: ChooseGreen();
                break;
            case 2: ChooseYellow();
                break;
            case 3: ChooseRed();
                break;
        }

        for (int i = 0; i < swords.Length; i++)
        {
            if(i == playerData.currentSwords)
            {
                swords[i].SetActive(true);
                swordsName.text = swords[i].name;
                currentSwords = i;
            }
            else
            {
                swords[i].SetActive(false);
            }
        }

        for (int i = 0; i < fastSwords.Length; i++)
        {
            if(i == playerData.currentFastSwords)
            {
                fastSwords[i].SetActive(true);
                fastSwordsName.text = fastSwords[i].name;
                currentFastSwords = i;
            }
            else
            {
                fastSwords[i].SetActive(false);
            }
        }

        for (int i = 0; i < hardSwords.Length; i++)
        {
            if(i == playerData.currentSword)
            {
                hardSwords[i].SetActive(true);
                swordName.text = hardSwords[i].name;
                currentSword = i;
            }
            else
            {
                hardSwords[i].SetActive(false);
            }
        }
    }

    private void ChooseSword()
    {
        slider.gameObject.SetActive(false);
        StartCoroutine(CameraTransition(swordCamera));
    }

    public void ChooseFastSwords()
    {
        slider.gameObject.SetActive(false);
        StartCoroutine(CameraTransition(fastSwordsCamera));
    }

    public void ChooseUsualSwords()
    {
        slider.gameObject.SetActive(false);
        StartCoroutine(CameraTransition(swordsCamera));
    }

    public void ChooseColor()
    {
        slider.gameObject.SetActive(true);
        StartCoroutine(CameraTransition(colorCamera));
    }

    public void ChooseMainMenu()
    {
        StartCoroutine(CameraTransition(menuCamera));
    }

    private void ChoosePurple()
    {
        skin.materials = new Material[] { playerData.body, playerData.cables, playerData.head, playerData.ribs };
        playerData.currentColor = 0;
    }
    private void ChooseGreen()
    {
        skin.materials = new Material[] { playerData.bodyGreen, playerData.cablesGreen, playerData.headGreen, playerData.ribsGreen };
        playerData.currentColor = 1;
    }
    private void ChooseYellow()
    {
        skin.materials = new Material[] { playerData.bodyYellow, playerData.cablesYellow, playerData.headYellow, playerData.ribsYellow };
        playerData.currentColor = 2;
    }

    private void ChooseRed()
    {
        skin.materials = new Material[] { playerData.bodyRed, playerData.cablesRed, playerData.headRed, playerData.ribsRed };
        playerData.currentColor = 3;
    }


    private void RotatePlayer(float value)
    {
        player.transform.rotation = Quaternion.Euler(0, value, 0);
    }

    private void ValidateAndDisplayName(string text)
    {
        if(string.IsNullOrWhiteSpace(text) || text.Length > 16)
        {
            return;
        }

        playerName.text = text;

        playerData.currentName = playerName.text;
    }

    private void RightFastSword()
    {
        currentFastSwords++;
        if(currentFastSwords == fastSwords.Length)
        {
            currentFastSwords = 0;
        }

        for (int i = 0; i < fastSwords.Length; i++)
        {
            if(i == currentFastSwords)
            {
                fastSwords[i].SetActive(true);
                fastSwordsName.text = fastSwords[i].name.ToString();
                playerData.currentFastSwords = i;
            }
            else
            {
                fastSwords[i].SetActive(false);
            }
        }
    }

    private void LeftFastSword()
    {
        currentFastSwords--;
        if(currentFastSwords < 0)
        {
            currentFastSwords = fastSwords.Length - 1;
        }

        for (int i = 0; i < fastSwords.Length; i++)
        {
            if(i == currentFastSwords)
            {
                fastSwords[i].SetActive(true);
                fastSwordsName.text = fastSwords[i].name.ToString();
                playerData.currentFastSwords = i;
            }
            else
            {
                fastSwords[i].SetActive(false);
            }
        }
    }

    private void RightSword()
    {
        currentSword++;

        if(currentSword == hardSwords.Length)
        {
            currentSword = 0;
        }

        for (int i = 0; i < hardSwords.Length; i++)
        {
            if(i == currentSword)
            {
                hardSwords[i].SetActive(true);
                swordName.text = hardSwords[i].name.ToString();
                playerData.currentSword = i;
            }
            else
            {
                hardSwords[i].SetActive(false);
            }
        }
    }

    private void LeftSword()
    {
        currentSword--;
        
        if(currentSword < 0)
        {
            currentSword = hardSwords.Length - 1;
        }

        for (int i = 0; i < hardSwords.Length; i++)
        {
            if(i == currentSword)
            {
                hardSwords[i].SetActive(true);
                swordName.text = hardSwords[i].name.ToString();
                playerData.currentSword = i;
            }
            else
            {
                hardSwords[i].SetActive(false);
            }
        }
    }

    private void RightSwords()
    {
        currentSwords++;

        if(currentSwords == swords.Length)
        {
            currentSwords = 0;
        }

        for (int i = 0; i < swords.Length; i++)
        {
            if(i == currentSwords)
            {
                swords[i].SetActive(true);
                swordsName.text = swords[i].name.ToString();
                playerData.currentSwords = i;
            }
            else
            {
                swords[i].SetActive(false);
            }
        }
    }

    private void LeftSwords()
    {
        currentSwords--;

        if(currentSwords < 0)
        {
            currentSwords = swords.Length - 1;
        }

        for (int i = 0; i < swords.Length; i++)
        {
            if(i == currentSwords)
            {
                swords[i].SetActive(true);
                swordsName.text = swords[i].name.ToString();
                playerData.currentSwords = i;
            }
            else
            {
                swords[i].SetActive(false);
            }
        }
    }

    public void HoverSoundButton()
    {
        AudioManager.Instance.Play("HoverButton");
    }

    public void ClickSoundButton()
    {
        AudioManager.Instance.Play("ClickButton");
    }

    private IEnumerator CameraTransition(Camera targetCamera)
    {
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        Vector3 targetPosition = targetCamera.transform.position;
        Quaternion targetRotation = targetCamera.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;
    }

    private void StartHost()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("Netcode is already running. Restarting...");
            NetworkManager.Singleton.Shutdown();
        }

        if (NetworkManager.Singleton.StartHost())
        {
            AudioManager.Instance.StopAllSounds();
            NetworkManager.Singleton.SceneManager.LoadScene("Arena", LoadSceneMode.Single);
        }
    }
    private void StartClient()
    {
        if (NetworkManager.Singleton.IsListening)
        {
            Debug.LogWarning("Network is already running. Restarting...");
            NetworkManager.Singleton.Shutdown();
        }

        AudioManager.Instance.StopAllSounds();
        NetworkManager.Singleton.StartClient();
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDisable()
    {
        hostButton.onClick.RemoveAllListeners();
        clientButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        colorButton.onClick.RemoveAllListeners();
        fastSwordsButton.onClick.RemoveAllListeners();
        swordButton.onClick.RemoveAllListeners();
        slowSwordsButton.onClick.RemoveAllListeners();

        backButton.onClick.RemoveAllListeners();

        fastRightArrowButton.onClick.RemoveAllListeners();
        fastLeftArrowButton.onClick.RemoveAllListeners();

        swordRightArrow.onClick.RemoveAllListeners();
        swordLeftArrow.onClick.RemoveAllListeners();

        swordsRightArrowButton.onClick.RemoveAllListeners();
        swordsLeftArrowButton.onClick.RemoveAllListeners();
    
        red.onClick.RemoveAllListeners();
        yellow.onClick.RemoveAllListeners();
        purple.onClick.RemoveAllListeners();
        green.onClick.RemoveAllListeners();

        slider.onValueChanged.RemoveAllListeners();
        inputField.onValueChanged.RemoveAllListeners();
    }
}
