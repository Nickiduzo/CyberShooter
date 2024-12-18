using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    private GameObject player;
    private CinemachineVirtualCamera cm;
    private void Awake()
    {
        cm = GetComponent<CinemachineVirtualCamera>();

        GameObject playerClone = GameObject.FindGameObjectWithTag("Player");
        if (playerClone != null)
        {
            player = playerClone;
            SetVariables();
        }
        else
        {
            Debug.LogWarning("I can't find player with tag 'Player'..");
        }
    }

    private void SetVariables()
    {
        cm.Follow = player.transform;
        cm.LookAt = player.transform;

        CinemachineCameraOffset offset = cm.GetComponent<CinemachineCameraOffset>();
        offset.m_Offset.x = -0.5f;
    }
}
