using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private bool isMoving;
    private bool isRuning;

    [SerializeField] private Animator anim;

    private float verticalInput;
    private float horizontalInput;

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
        isMoving = horizontalInput != 0 || verticalInput != 0;
        anim.SetBool("isMove", isMoving);
    }

    private void RunAnimation()
    {
        isRuning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        anim.SetBool("isRun", isRuning);
    }

    private void PlayJumpAnimation() => anim.SetTrigger("isJump");
    private void PlayFallAnimation() => anim.SetTrigger("isFall");
    private void PlayLandAnimation() => anim.SetTrigger("isLand");

    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G)) anim.SetTrigger("Dance");
    }

    private void OnDisable()
    {
        
    }
}
