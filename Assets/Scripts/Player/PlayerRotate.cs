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

    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            UpdateRotationServerRpc(transform.rotation);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            HandlerRotation();
        }
    }

    private void HandlerRotation()
    {
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;

        transform.Rotate(Vector3.up, horizontalInput);

        UpdateRotationServerRpc(transform.rotation);
    }

    [ServerRpc]
    private void UpdateRotationServerRpc(Quaternion newRotation)
    {
        rotation.Value = newRotation;
    }
}
