using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private PlayerAttackTimer attackTimer;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float sprintSpeed = 9f;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementPosition;

    private NetworkVariable<Vector3> playerPosition = new NetworkVariable<Vector3>();

    private void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        if (!attackTimer.GetKick())
        {
            Move();
        }
    }

    private void Move()
    {
        if (!IsGrounded()) return;

        float currentSpeed = walkSpeed;
        
        if(Input.GetKey(KeyCode.LeftAlt)) currentSpeed = runSpeed;
        
        if(Input.GetKey(KeyCode.LeftShift)) currentSpeed = sprintSpeed;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        movementPosition = new Vector3(horizontalInput, 0, verticalInput).normalized;

        Vector3 moveDirection = transform.TransformDirection(movementPosition) * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);

        UpdateMovementServerRpc(rb.position);
    }

    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + Vector3.down * 0.2f, 0.3f);
    }

    [ServerRpc]
    private void UpdateMovementServerRpc(Vector3 newPosition)
    {
        playerPosition.Value = newPosition;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        Spawn();
    }

    public void Spawn()
    {
        Vector3 newPosition = playerData.GetRandomPosition();

        rb.MovePosition(newPosition);

        UpdateMovementServerRpc(rb.position);
    }
}
