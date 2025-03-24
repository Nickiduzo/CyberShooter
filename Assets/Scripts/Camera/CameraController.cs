using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        playerCamera.gameObject.SetActive(IsOwner);
        virtualCamera.gameObject.SetActive(IsOwner);
        audioListener.enabled = IsOwner;
    }
}
