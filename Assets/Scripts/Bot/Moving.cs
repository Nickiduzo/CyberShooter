using UnityEngine;
public class Moving : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float jumpForce = 5f;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    private bool isGrounded;
    [SerializeField] private float checkDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        print(isGrounded);
        HandleInput();
        AnimateMovement();
        RotatePlayer();
    }

    private void FixedUpdate()
    {
        Move();
        GroundCheck();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("Dance");
        }
    }

    private void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Преобразуем направление движения в мировые координаты
        Vector3 moveDirection = transform.TransformDirection(direction) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // Сбросим состояние при прыжке
    }

    private void GroundCheck()
    {
        // Используем Raycast для проверки касания земли
        isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance, groundLayer);
    }

    private void AnimateMovement()
    {
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        anim.SetBool("isMove", isMoving);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        anim.SetBool("isRun", isRunning);
    }

    private void RotatePlayer()
    {
        // Получаем вращение по оси Y для поворота персонажа
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Вращаем камеру по оси X (вверх-вниз)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -lookLimit, lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }
}
