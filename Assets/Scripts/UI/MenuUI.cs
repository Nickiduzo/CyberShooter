using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Cursor.visible = true;

        hostButton.onClick.AddListener(StartHost);
        clientButton.onClick.AddListener(StartClient);

        exitButton.onClick.AddListener(ExitGame);
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
}
