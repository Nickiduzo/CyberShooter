using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerJump : NetworkBehaviour
{
    public static event Action OnLand;

    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private PlayerAnimation playerAnimation;

    private float timeInAir = 0f;
    private float cooldown = 2f;

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

        if (IsGrounded())
        {
            timeInAir = 0;
            cooldown -= Time.deltaTime;
        }
        else
        {
            timeInAir += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded() && cooldown <= 0)
        {
            Vector3 jumpDirection = transform.forward + new Vector3(0,2,0);
            jumpDirection.Normalize();

            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            cooldown = 2f;

            playerAnimation.JumpServerRpc();
        }
    }

    private void CheckLanding()
    {
        if(timeInAir > 0.1f && IsGrounded())
        {
            OnLand?.Invoke();
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.2f);
    }
}
