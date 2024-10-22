using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerBehaviour))]
public class PlayerSwordAttack : MonoBehaviour
{
    [SerializeField] private PlayerState currentState;

    [SerializeField] private PlayerBehaviour playerBehaviour;

    //[SerializeField] private ParticleSystem attackEffect;

    public UnityEvent<int> PlayAnimation;

    private void Update()
    {
        currentState = playerBehaviour.currentState;
        HandlerAnimation();
    }
    private void HandlerAnimation()
    {
        if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
        {
            ChargeAttack();
        }
        else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1))
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
        if (currentState == PlayerState.Melee)
        {
            print("Make Kick");
            PlayAnimation?.Invoke(1);
        }
    }
    
    private void DoubleAttack()
    {
        if(currentState == PlayerState.Melee)
        {
            print("Double attack");
            PlayAnimation?.Invoke(2);
        }
    }

    private void ChargeAttack()
    {
        if(currentState == PlayerState.Melee)
        {
            print("Charge attack");
            PlayAnimation.Invoke(3);
        }
    }

    private void ChargeSprintAttack()
    {
        if (currentState == PlayerState.Melee)
        {
            print("Charge sprint attack");
            PlayAnimation.Invoke(4);
        }
    }
}
