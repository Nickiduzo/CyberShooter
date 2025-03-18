using System;
using Unity.Netcode;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static event Action<int> ShowDamage;
    public static event Action SwordHit;

    public bool canDamage = false;

    public int swordDamage = 25;

    [SerializeField] private PlayerAnimation playerAnimation;
    private void Start()
    {
        playerAnimation.ActivateAttack.AddListener(SwordOn);
        playerAnimation.DeactivateAttack.AddListener(SwordOff);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent(out PlayerHp playerHp))
            {
                ShowDamage?.Invoke(swordDamage);
                SwordHit?.Invoke();
                playerHp.TakeDamage(swordDamage, NetworkManager.Singleton.LocalClientId);
            }
        }
    }


    private void SwordOn() => canDamage = true;
    private void SwordOff() => canDamage = false;
}
