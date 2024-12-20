using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalLookRotation = 0f;
    [SerializeField] private float lookLimit = 85f;

    private NetworkVariable<Quaternion> playerRotation = new NetworkVariable<Quaternion>();

    public override void OnNetworkSpawn()
    {
        InitializeCamera();

        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!IsOwner) return;

        RotatePlayer();
    }

    private void InitializeCamera()
    {
        if (!IsOwner)
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

    private void RotatePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -lookLimit, lookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);

        UpdateCameraRotationServerRpc(transform.rotation);
    }


    [ServerRpc]
    private void UpdateCameraRotationServerRpc(Quaternion newVerticalRotation)
    {
        playerRotation.Value = newVerticalRotation;
    }
}
