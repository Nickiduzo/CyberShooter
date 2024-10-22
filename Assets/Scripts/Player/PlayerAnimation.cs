using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private bool isMoving;
    private bool isRuning;

    private float verticalInput;
    private float horizontalInput;

    private bool isGrounded = false;
    private void Start()
    {
        InitializeListeners();
    }

    private void Update()
    {
        anim.SetBool("Grounded", isGrounded);

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        Dance();
        MoveBehaviour();
    }

    private void InitializeListeners()
    {
        PlayerJump playerJump = GetComponent<PlayerJump>();
        if(playerJump != null)
        {
            playerJump.OnJump.AddListener(PlayJumpAnimation);
            
        }

        PlayerSwordAttack playerSword = GetComponent<PlayerSwordAttack>();
        if(playerSword != null)
        {
            playerSword.PlayAnimation.AddListener(DetectSwordAnimation);
        }

        PlayerRifleShot playerRifle = GetComponent<PlayerRifleShot>();
        if(playerRifle != null )
        {
            playerRifle.MakeShot.AddListener(ShotAnimation);
        }
    }

    private void MoveBehaviour()
    {
        MoveAnimation();
        RunAnimation();
    }
    private void MoveAnimation()
    {
        isMoving = horizontalInput != 0 || verticalInput != 0;
        anim.SetBool("isMove", isMoving);
    }

    private void RunAnimation()
    {
        isRuning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        anim.SetBool("isRun", isRuning);
    }

    private void PlayJumpAnimation()
    {
        anim.SetBool("isJump", true);
    }

    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G)) anim.SetTrigger("Dance");
    }

    private void ShotAnimation()
    {
        anim.SetTrigger("isShot");
    }

    private void DetectSwordAnimation(int animationIndex)
    {
        switch (animationIndex)
        {
            case 1:SwordAttack();
                break;
            case 2:SwordDoubleAttack();
                break;
            case 3:SwordChargeAttack();
                break;
            case 4:SwordChargeDoubleAttack();
                break;
            default: print("Ты лох");
                break;
        }
    }
    private void SwordAttack()
    {
        anim.SetTrigger("isSword");
    }

    private void SwordDoubleAttack()
    {
        anim.SetTrigger("isSwordDouble");
    }

    private void SwordChargeAttack()
    {
        anim.SetTrigger("isSwordCharge");
    }

    private void SwordChargeDoubleAttack()
    {
        anim.SetTrigger("isSwordChargeDouble");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Building")) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
