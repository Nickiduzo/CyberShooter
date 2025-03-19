using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerJump : NetworkBehaviour
{
    public static event Action OnLand;

    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private PlayerAnimation playerAnimation;

    private bool wasInAir = false;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        Jump();
        CheckLanding();
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector3 jumpDirection = transform.forward + new Vector3(0,2,0);
            jumpDirection.Normalize();

            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            wasInAir = true;
            
            playerAnimation.JumpServerRpc();
        }
    }

    private void CheckLanding()
    {
        if(wasInAir && IsGrounded() && !Input.GetKeyDown(KeyCode.Space))
        {
            wasInAir = false;
            OnLand?.Invoke();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }
}
