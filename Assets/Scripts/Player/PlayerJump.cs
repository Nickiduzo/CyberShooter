using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 3f;

    public UnityEvent OnJump;

    private bool isGrounded = false;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Jump();
        CheckFall();
    }

    private void Jump()
    {
        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            AudioManager.instanse.Play("Jump");
            OnJump?.Invoke();
        }
    }

    private void CheckFall()
    {
        if (!isGrounded && rb.velocity.y < 0)
        {
            //OnFall?.Invoke();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //OnLand?.Invoke();
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
