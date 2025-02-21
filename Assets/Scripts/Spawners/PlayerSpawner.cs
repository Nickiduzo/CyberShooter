using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPositions;

    private int nextSpawnIndex = 0;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        GameObject playerObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject.gameObject;

        if (playerObject != null)
        {
            if(nextSpawnIndex >= spawnPositions.Length)
            {
                nextSpawnIndex = 0;
            }

            Transform spawnPoint = spawnPositions[nextSpawnIndex++];

            Debug.LogWarning("Spawned player at: " + spawnPoint);

            playerObject.transform.position = spawnPoint.position;
        }

        print("Didn't change player position");
    }
}
