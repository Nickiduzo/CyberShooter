using TMPro;
using UnityEngine;

public class RuneTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;

    private bool isRune = false;
    private float timerOfRune = 10f;
    private void Start()
    {
        Rune.IncreaseSpeed += ShowTimer;
    }

    private void Update()
    {
        if(isRune)
        {
            timerOfRune -= Time.deltaTime;
            if(timerOfRune <= 0) 
            {
                timer.gameObject.SetActive(false);
            }
            timer.text = timerOfRune.ToString();
        }
    }

    private void ShowTimer()
    {
        isRune = true;
        timer.gameObject.SetActive(true);
        timerOfRune = 10f;
    }

    private void OnDestroy()
    {
        Rune.IncreaseSpeed -= ShowTimer;
    }
}
