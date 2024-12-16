using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void LaunchGame(int index) => SceneManager.LoadScene(index);
}
