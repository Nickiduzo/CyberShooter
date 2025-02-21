using Unity.Netcode;
using UnityEngine;

public class PlayerRotate : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    private NetworkVariable<Quaternion> rotation = new NetworkVariable<Quaternion>(
        Quaternion.identity,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
        );

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        HandlerRotation();
    }

    private void HandlerRotation()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, horizontalInput);

        if(Quaternion.Angle(transform.rotation, rotation.Value) > 0.1f)
        {
            UpdateRotationServerRpc(transform.rotation);
        }
    }

    [ServerRpc]
    private void UpdateRotationServerRpc(Quaternion newRotation)
    {
        rotation.Value = newRotation;
    }
}
