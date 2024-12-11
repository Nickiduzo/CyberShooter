using System;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public static event Action IncreaseSpeed;

    private float timeToSpawn;

    private void Start()
    {
        timeToSpawn = 30f;
    }

    private void Update()
    {
        if(timeToSpawn <= 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            timeToSpawn -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            print("Player took rune of speed");
            IncreaseSpeed?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
