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
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        Quaternion newRotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + horizontalInput ,0);
        
        UpdateRotationServerRpc(newRotation);
    }

    [ServerRpc]
    private void UpdateRotationServerRpc(Quaternion newRotation)
    {
        rotation.Value = newRotation;
        UpdateRotationClientRpc(newRotation);
    }

    [ClientRpc]
    private void UpdateRotationClientRpc(Quaternion newRotation)
    {
        if(IsOwner)
        {
            transform.rotation = newRotation;
        }
    }
}
