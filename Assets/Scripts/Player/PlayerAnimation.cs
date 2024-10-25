using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    
    private bool isMoving;
    private bool isRuning;

    private float x;
    private float y;

    private bool isGrounded = false;
    private void Start()
    {
        InitializeListeners();
    }

    private void Update()
    {
        anim.SetBool("Grounded", isGrounded);

        Dance();
        MoveBehaviour();
    }

    private void MoveBehaviour()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if(y >= 0.6f && !Input.GetKey(KeyCode.LeftShift))
        {
            y = 0.6f;
        }
        else if(y == 0.6f && Input.GetKey(KeyCode.LeftShift))
        {
            y = 1f;
        }


        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

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

    private void PlayJumpAnimation()
    {
        anim.SetBool("isJump", true);
    }

    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.SetTrigger("Dance");
            AudioManager.instanse.Play("Polskaya");
        }
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
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Building"))
        {
            isGrounded = true;
            anim.SetBool("isJump", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Building"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
