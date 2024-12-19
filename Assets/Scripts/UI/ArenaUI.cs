using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(ExitFromArena);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }

    private void ExitFromArena()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
        else if(NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene("Menu");
    }
}
