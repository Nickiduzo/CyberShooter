using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMove : NetworkBehaviour
{
    [SerializeField] private PlayerAttackTimer attackTimer;

    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float sprintSpeed = 9f;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private bool isKick = false;

    [SerializeField] private Rigidbody rb;
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

        isKick = attackTimer.GetKick();

        if (!isKick)
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
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    [ServerRpc]
    private void UpdateMovementServerRpc(Vector3 newPosition)
    {
        playerPosition.Value = newPosition;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        float randZ = UnityEngine.Random.RandomRange(0, 5f);

        Vector3 newPosition = new Vector3(randZ, 2.5f, randZ);

        rb.MovePosition(rb.position + newPosition);
        UpdateMovementServerRpc(rb.position);
    }
}
