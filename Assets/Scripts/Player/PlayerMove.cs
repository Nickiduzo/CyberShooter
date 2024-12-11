using System;
using UnityEditor;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 6f;

    private float walkRuneSpeed = 6f;
    private float sprintRuneSpeed = 12f;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    private float timerOfSpeedRune = 0f;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        Rune.IncreaseSpeed += IncreaseSpeed;
    }

    private void Update()
    {
        HandleInput();
        RotatePlayer();

        RuneHandler();
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

    private void IncreaseSpeed()
    {
        timerOfSpeedRune = 10;

        sprintSpeed = sprintRuneSpeed;
        walkSpeed = walkRuneSpeed;
    }

    private void RuneHandler()
    {
        if (timerOfSpeedRune <= 0)
        {
            sprintSpeed = 6f;
            walkSpeed = 3f;
        }
        else
        {
            timerOfSpeedRune -= Time.deltaTime;
        }
    }

    private void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        Vector3 moveDirection = transform.TransformDirection(direction) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
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

    private void OnDestroy()
    {
        Rune.IncreaseSpeed -= IncreaseSpeed;   
    }
}
