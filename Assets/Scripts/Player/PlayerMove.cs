using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera virtualCamera;

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
    private Vector3 movementPosition;

    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>(
            writePerm: NetworkVariableWritePermission.Server);

    private NetworkVariable<Quaternion> playerRotation = new NetworkVariable<Quaternion>(
        writePerm: NetworkVariableWritePermission.Server);


    private void Awake()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if(!IsOwner)
        {
            virtualCamera.Priority = 0;
            virtualCamera.gameObject.SetActive(false);
        }
        else
        {
            virtualCamera.Priority = 10;
            virtualCamera.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleInput();
        RotatePlayer();

        isKick = attackTimer.GetKick();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if(!isKick)
        {
            Move();
        }
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");    
        
        movementPosition = new Vector3(horizontalInput, 0,verticalInput).normalized;
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

        Vector3 moveDirection = transform.TransformDirection(movementPosition) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        UpdateMovementServerRpc(rb.position, transform.rotation);
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

    [ServerRpc]
    private void UpdateMovementServerRpc(Vector3 newPosition, Quaternion newRotation)
    {
        playerPosition.Value = newPosition;
        playerRotation.Value = newRotation;
    }

    [ClientRpc]
    private  void UpdateMovementClientRpc(Vector3 newPosition, Quaternion newRotation)
    {
        if(!IsOwner)
        {
            rb.position = newPosition;
            playerRotation.Value = newRotation;
        }
    }

    private void OnNetworkSpawn()
    {
        if(!IsOwner)
        {
            playerPosition.OnValueChanged += (oldPos, newPos) => rb.position = newPos;
            playerRotation.OnValueChanged += (oldRot, newRot) => transform.rotation = newRot;
        }
    }
}
