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
        PlayerJump playerJump = GetComponent<PlayerJump>();
        if(playerJump != null)
        {
            playerJump.OnJump.AddListener(PlayJumpAnimation);
        }
    }

    private void Update()
    {
        anim.SetBool("Grounded", isGrounded);

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        Dance();
        MoveBehaviour();
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
