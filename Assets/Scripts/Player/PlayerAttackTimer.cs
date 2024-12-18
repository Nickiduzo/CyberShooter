using Unity.Netcode;
using UnityEngine;

[RequireComponent (typeof(PlayerBehaviour))]
public class PlayerAttackTimer : NetworkBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [Header("Two swords")]
    [SerializeField][Range(0,2f)] private float swordsAttack;
    [SerializeField][Range(0,2f)] private float swordsDoubleAttack;
    [SerializeField][Range(0,3.5f)] private float swordsChargeAttack;
    [SerializeField][Range(0,3.5f)] private float swordsDChargeAttack;

    [Header("Two Fast Swords")]
    [SerializeField][Range(0, 3f)] private float fastAttack;
    [SerializeField][Range(0, 3f)] private float doubleAttack;
    [SerializeField][Range(0, 3f)] private float chargeAttack;
    [SerializeField][Range(0, 3f)] private float dChargeAttack;

    [Header("One sword")]
    [SerializeField][Range(0, 7.5f)] private float swordAttack;
    [SerializeField][Range(0, 7.5f)] private float swordHard;
    [SerializeField][Range(0, 10f)] private float swordChargeAttack;
    [SerializeField][Range(0, 10f)] private float swordJumpAttack;

    private float currentTimer;

    private PlayerState playerState;

    public bool isKick = false;
    private void Start()
    {
        SetState();
    }

    private void Update()
    {
        if (!IsOwner) return;

        SetState();
        HandlerAttack();

        if(currentTimer <= 0)
        {
            isKick = false;
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }
    }

    private void HandlerAttack()
    {
        if(playerState == PlayerState.TwoSwords)
        {
            isKick = true;

            if (Input.GetKeyDown(KeyCode.W))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    currentTimer = swordsChargeAttack;
                }

                if(Input.GetMouseButtonDown(1))
                {
                    currentTimer = swordsDChargeAttack;
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                currentTimer = swordsAttack;
            }

            if(Input.GetMouseButtonDown(1))
            {
                currentTimer = swordsDoubleAttack;
            }
        }
    
        if(playerState == PlayerState.Sword)
        {
            isKick = true;

            if(Input.GetKeyDown(KeyCode.W))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    currentTimer = swordChargeAttack;
                }

                if(Input.GetMouseButtonDown(1))
                {
                    currentTimer = swordJumpAttack;
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                currentTimer = swordAttack;
            }

            if(Input.GetMouseButtonDown(1))
            {
                currentTimer = swordHard;
            }
        }

        if(playerState == PlayerState.FastSwords)
        {
            isKick = true;

            if(Input.GetKeyDown(KeyCode.W))
            {
                if(Input.GetMouseButtonDown(0))
                {
                    currentTimer = chargeAttack;
                }

                if(Input.GetMouseButtonDown(1))
                {
                    currentTimer = dChargeAttack;
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                currentTimer = fastAttack;
            }

            if(Input.GetMouseButtonDown(1))
            {
                currentTimer = doubleAttack;
            }
        }
    }


    public bool GetKick()
    {
        return isKick;
    }

    private void SetState() => playerState = playerBehaviour.currentState;
}
