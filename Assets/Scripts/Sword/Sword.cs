using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    public UnityEvent<int> OnHit;

    private int damage = 25;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            OnHit?.Invoke(damage);
        }
    }
}
