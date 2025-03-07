using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerBehaviour))]
public class PlayerSwordAttack : NetworkBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private PlayerAttackTimer attackCooldown;

    private PlayerState currentState;

    [HideInInspector] public UnityEvent<int> PlayAnimation;

    private void Update()
    {
        if (!IsOwner) return;

        currentState = playerBehaviour.currentState;
        HandlerAnimation();
    }
    private void HandlerAnimation()
    {
        if (attackCooldown.GetKick()) return;
        if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
        {
            ChargeAttack();
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1))
        {
            ChargeSprintAttack();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DoubleAttack();
        }
    }

    private void Attack()
    {
        if (currentState == PlayerState.TwoSwords || currentState == PlayerState.FastSwords)
        {
            PlayAnimation?.Invoke(1);
        }

        if(currentState == PlayerState.Sword)
        {
            PlayAnimation?.Invoke(5);
        }
    }
    
    private void DoubleAttack()
    {
        if(currentState == PlayerState.TwoSwords || currentState == PlayerState.FastSwords)
        {
            PlayAnimation?.Invoke(2);
        }

        if(currentState == PlayerState.Sword)
        {
            PlayAnimation?.Invoke(6);
        }
    }

    private void ChargeAttack()
    {
        if(currentState == PlayerState.TwoSwords || currentState == PlayerState.FastSwords)
        {
            PlayAnimation.Invoke(3);
        }

        if (currentState == PlayerState.Sword)
        {
            PlayAnimation?.Invoke(7);
        }
    }

    private void ChargeSprintAttack()
    {
        if (currentState == PlayerState.TwoSwords || currentState == PlayerState.FastSwords)
        {
            PlayAnimation.Invoke(4);
        }

        if (currentState == PlayerState.Sword)
        {
            PlayAnimation?.Invoke(8);
        }
    }
}
