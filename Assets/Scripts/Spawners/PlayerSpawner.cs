using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] greenSpawnPositions;
    [SerializeField] private Transform[] redSpawnPositions;

    private void SpawnPlayer(GameObject playerPrefab, ulong clientId)
    {
        Transform spawnPoint = greenSpawnPositions[(int)(clientId % (ulong)greenSpawnPositions.Length)];
        
        print(spawnPoint);

        if (IsServer)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }

    }
}
