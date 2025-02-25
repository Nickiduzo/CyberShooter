using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class PlayerAnimation : NetworkBehaviour
{
    [HideInInspector] public UnityEvent ActivateAttack;
    [HideInInspector] public UnityEvent DeactivateAttack;

    [SerializeField] private Animator anim;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private PlayerSwordAttack playerSwordAttack;

    private float x;
    private float y;

    private bool isGrounded;

    private NetworkVariable<float> networkX = new NetworkVariable<float>();
    private NetworkVariable<float> networkY = new NetworkVariable<float>();
    private NetworkVariable<bool> networkIsMoving = new NetworkVariable<bool>();
    private NetworkVariable<bool> networkJump = new NetworkVariable<bool>();
    private void Awake()
    {
        InitializeListeners();
    }

    private void Update()
    {
        if (!IsOwner) return;

        anim.SetBool("Grounded", isGrounded);
        Dance();
        StartMoving();
        MoveBehaviour();
    }

    [ServerRpc]
    private void SetAnimServerRpc(float x, float y, float speedFactor)
    {
        networkX.Value = x;
        networkY.Value = y;
        networkIsMoving.Value = (x != 0 || y != 0);
        SetAnimClientRpc(x, y, speedFactor);
    }

    [ClientRpc]
    private void SetAnimClientRpc(float x, float y, float speedFactor)
    {
        if(!IsOwner)
        {
            anim.SetFloat("x", x * speedFactor);
            anim.SetFloat("y", y * speedFactor);
        }
    }

    private void MoveBehaviour()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        float speedFactor = 0.1f;

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            speedFactor = 0.6f;
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            speedFactor = 1f;
        }

        anim.SetFloat("x", x * speedFactor);
        anim.SetFloat("y", y * speedFactor);

        SetAnimServerRpc(x, y, speedFactor);
    }

    private void InitializeListeners()
    {
        playerSwordAttack.PlayAnimation.AddListener(DetectSwordAnimation);
    }
    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G) && x == 0 && y == 0 && playerBehaviour.currentState == PlayerState.Empty)
        {
            anim.SetTrigger("Dance");
            PerformDanceServerRpc("Dance");
        }

        if(Input.GetKeyDown(KeyCode.H) && x == 0 && y == 0 && playerBehaviour.currentState == PlayerState.Empty)
        {
            anim.SetTrigger("Nyan");
            PerformDanceServerRpc("Nyan");
        }

        if(Input.GetKeyDown(KeyCode.J) && x == 0 && y == 0 && playerBehaviour.currentState == PlayerState.Empty)
        {
            anim.SetTrigger("Best");
            PerformDanceServerRpc("Best");
        }
    }


    [ServerRpc]
    public void JumpServerRpc()
    {
        networkJump.Value = true;
        MakeJumpClientRpc();
    }

    [ClientRpc]
    private void MakeJumpClientRpc()
    {
        anim.SetTrigger("isJump");
    }

    public void DetectSwordAnimation(int animationIndex)
    {
            switch (animationIndex)
            {
                case 1:
                    SwordAttack();
                    break;
                case 2:
                    SwordDoubleAttack();
                    break;
                case 3:
                    SwordChargeAttack();
                    break;
                case 4:
                    SwordChargeDoubleAttack();
                    break;
                case 5:
                    SwordAttack();
                    break;
                case 6:
                    SwordDoubleAttack();
                    break;
                case 7:
                    SwordChargeAttack();
                    break;
                case 8:
                    SwordChargeDoubleAttack();
                    break;
            }
    }
    private void SwordAttack()
    {
        anim.SetTrigger("isSword");
        PerformSwordAttackServerRpc("isSword");
    }

    private void SwordDoubleAttack()
    {
        anim.SetTrigger("isSwordDouble");
        PerformSwordAttackServerRpc("isSwordDouble");
    }

    private void SwordChargeAttack()
    {
        anim.SetTrigger("isSwordCharge");
        PerformSwordAttackServerRpc("isSwordCharge");
    }

    private void SwordChargeDoubleAttack()
    {
        anim.SetTrigger("isSwordChargeDouble");
        PerformSwordAttackServerRpc("isSwordChargeDouble");
    }
    public void SwordAttackOn()
    {
        ActivateAttack?.Invoke();
    }

    public void SwordAttackOff()
    {
        DeactivateAttack?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") 
            || collision.gameObject.CompareTag("Building"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;  
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void StartMoving()
    {
        if(x != 0 || y != 0)
        {
            anim.SetBool("isMove",true);
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    [ServerRpc]
    private void PerformSwordAttackServerRpc(string triggerName)
    {
        TriggerSwordAttackClientRpc(triggerName);
    }

    [ClientRpc]
    private void TriggerSwordAttackClientRpc(string triggerName)
    {
        if(!IsOwner)
        {
            anim.SetTrigger(triggerName);
        }
    }

    [ServerRpc]
    private void PerformDanceServerRpc(string triggerName)
    {
        TriggerDanceClientRpc(triggerName);
    }

    [ClientRpc]
    private void TriggerDanceClientRpc(string triggerName)
    {
        if (!IsOwner) anim.SetTrigger(triggerName);
    }
}
