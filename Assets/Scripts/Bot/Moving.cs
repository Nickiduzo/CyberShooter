using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float mouseSensetivity = 100f;
    [SerializeField] private float lookLimit = 85f;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float shiftSpeed = 6f;

    private float verticalLookRotation = 0f;

    private float horizontalInput;
    private float verticalInput;
    private void Update()
    {
        Move();
        ChangeAnimation();
        RotateBot();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * speed * verticalInput * Time.deltaTime);
        transform.Translate(Vector3.right * speed * horizontalInput * Time.deltaTime);
    }

    private void ChangeAnimation()
    {
        if(horizontalInput != 0 || verticalInput != 0)
        {
            anim.SetBool("isMoving", true);
        }

        if(horizontalInput == 0 && verticalInput == 0)
        {
            anim.SetBool("isMoving", false);
        }
    }

    // rotation of player
    private void RotateBot()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.fixedDeltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation,-lookLimit,lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }
}
