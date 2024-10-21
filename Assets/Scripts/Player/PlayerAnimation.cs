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
            playerJump.OnFall.AddListener(PlayFallAnimation);
            playerJump.OnLand.AddListener(PlayLandAnimation);
        }
    }

    private void Update()
    {
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
        if (isGrounded)
        {
            isMoving = horizontalInput != 0 || verticalInput != 0;
            anim.SetBool("isMove", isMoving);
        }
    }

    private void RunAnimation()
    {
        if (isGrounded)
        {
            isRuning = Input.GetKey(KeyCode.LeftShift) && isMoving;
            anim.SetBool("isRun", isRuning);
        }
    }

    private void PlayJumpAnimation() => anim.SetTrigger("isJump");
    private void PlayFallAnimation() => anim.SetTrigger("isFall");
    private void PlayLandAnimation() => anim.SetTrigger("isLand");

    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G) && isGrounded) anim.SetTrigger("Dance");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
