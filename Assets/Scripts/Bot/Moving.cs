using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float mouseSensetivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float jumpHeight = 2f;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    private bool isGrounded = true;
    [SerializeField] private float checkDistance;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Rigidbody rb;
    

    private void Update()
    {
        //CheckOnGround();
        //Jump();
        Move();
        ChangeAnimation();
        RotateBot();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        if(speed == sprintSpeed && IsMoving())
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }
    
        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
    }

    //private void Jump()
    //{
    //    if(Input.GetButtonDown("Jump") && isGrounded)
    //    {
    //        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
    //        isGrounded = false;
    //    }
    //}

    //private void CheckOnGround()
    //{
    //    isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance, groundLayer);
    //    if(isGrounded)
    //    {
    //        anim.SetBool("isJump", true);
    //    }
    //    else
    //    {
    //        anim.SetBool("isJump", false);
    //    }
    //}
    private void ChangeAnimation()
    {
        if(horizontalInput != 0 || verticalInput != 0)
        {
            anim.SetBool("isMove", true);
        }

        if(horizontalInput == 0 && verticalInput == 0)
        {
            anim.SetBool("isMove", false);
        }
    }
    private bool IsMoving()
    {
        return (horizontalInput != 0 || verticalInput != 0);
    }
    // rotation of player
    private void RotateBot()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.fixedDeltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation,-lookLimit,lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }
}
