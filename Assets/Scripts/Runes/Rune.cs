using System;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public static event Action IncreaseSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            print("Player took rune of speed");
            IncreaseSpeed?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
