using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArenaUI : MonoBehaviour
{
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(() =>
        {
            LaunchMenu();
        });
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
        }
    }

    private void LaunchMenu() => SceneManager.LoadScene(0);
}
