using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    public static event Action<int> ShowDamage;

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
                playerHp.TakeDamage(swordDamage, NetworkManager.Singleton.LocalClientId);
            }
        }
    }


    private void SwordOn() => canDamage = true;
    private void SwordOff() => canDamage = false;
}
