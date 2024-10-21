using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;

    private bool isGrounded = false;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    private PlayerState state;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        state = PlayerState.Melee;
    }

    private void Update()
    {
        HandleInput();
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void Move()
    {
        if (isGrounded)
        {
            float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

            Vector3 moveDirection = transform.TransformDirection(direction) * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);
        }
    }

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -lookLimit, lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
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
