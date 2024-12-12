using UnityEngine;

[RequireComponent (typeof(PlayerBehaviour))]
public class PlayerAttackTimer : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [Header("Two swords")]
    [SerializeField][Range(0,2f)] private float swordsAttack;
    [SerializeField][Range(0,2f)] private float swordsDoubleAttack;
    [SerializeField][Range(0,2f)] private float swordsChargeAttack;
    [SerializeField][Range(0,2f)] private float swordsDChargeAttack;

    [Header("One sword")]
    [SerializeField][Range(0, 5f)] private float swordAttack;
    [SerializeField][Range(0, 5f)] private float swordHard;
    [SerializeField][Range(0, 5f)] private float swordChargeAttack;
    [SerializeField][Range(0, 5f)] private float swordJumpAttack;

    private float currentTimer;

    private PlayerState playerState;

    public bool isKick = false;
    private void Start()
    {
        SetState();
    }

    private void Update()
    {
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
    }


    public bool GetKick()
    {
        return isKick;
    }

    private void SetState() => playerState = playerBehaviour.currentState;
}
