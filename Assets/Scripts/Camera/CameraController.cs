using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] private Camera playerCamera;
    private AudioListener audioListener;

    private void Start()
    {
        audioListener = GetComponent<AudioListener>();

        if(IsOwner)
        {
            playerCamera.gameObject.SetActive(true);
            if(audioListener != null)
            {
                audioListener.enabled = false;
            }
        }
        else
        {
            playerCamera.gameObject.SetActive(false);
            if(audioListener != null)
            {
                audioListener.enabled = false;
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if(!IsOwner)
        {
            playerCamera.gameObject.SetActive(false);
            if (audioListener != null)
            {
                audioListener.enabled = false;
            }
        }
    }
}
