using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button serverButton;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            LaunchGame(1);
        });

        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            LaunchGame(1);
        });

        serverButton.onClick.AddListener(() =>
        {
            LaunchGame(1);
            NetworkManager.Singleton.StartServer();
        });
    }

    public void LaunchGame(int index) => SceneManager.LoadScene(index);

    public void ExitGame()
    {
        print("Exit from game");
    }
}
