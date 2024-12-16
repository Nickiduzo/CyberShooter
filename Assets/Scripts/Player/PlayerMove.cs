using System;
using UnityEditor;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private PlayerAttackTimer attackTimer;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float sprintSpeed = 9f;

    private float walkRuneSpeed = 5f;
    private float runRuneSpeed = 10f;
    private float sprintRuneSpeed = 15f;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;

    private float timerOfSpeedRune = 0f;

    [SerializeField] private bool isKick = false;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        Rune.IncreaseSpeed += IncreaseSpeed;
    }

    private void Update()
    {
        isKick = attackTimer.GetKick();

        HandleInput();
        RotatePlayer();

        RuneHandler();
    }

    private void FixedUpdate()
    {
        if(!isKick)
        {
            Move();
        }
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
        runSpeed = runRuneSpeed;
        walkSpeed = walkRuneSpeed;
    }

    private void RuneHandler()
    {
        if (timerOfSpeedRune <= 0)
        {
            sprintSpeed = 9f;
            runSpeed = 6f;
            walkSpeed = 3f;
        }
        else
        {
            timerOfSpeedRune -= Time.deltaTime;
        }
    }

    private void Move()
    {
        float speed = walkSpeed;
        
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            speed = runSpeed;
        }
        
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintSpeed;
        }


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
