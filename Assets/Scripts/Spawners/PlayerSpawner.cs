using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    private Dictionary<ulong, PlayerStats> players = new Dictionary<ulong, PlayerStats>();

    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += PlayerConnectionHandler;
            NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnectionHandler;

            ulong hostId = NetworkManager.Singleton.LocalClientId;
            PlayerConnectionHandler(hostId);
        }
    }

    private void PlayerConnectionHandler(ulong playerId)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(playerId)) return;

        PlayerStats player = NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            players[playerId] = player;
        }
    }

    private void PlayerDisconnectionHandler(ulong playerId)
    {
        if (!NetworkManager.Singleton.IsServer) return;

        if (playerId == NetworkManager.Singleton.LocalClientId) return;

        if (players.ContainsKey(playerId))
        {
            players.Remove(playerId);
        }
    }

    public List<PlayerStats> GetAllPlayers()
    {
        return new List<PlayerStats>(players.Values);
    }

    private void OnDisable()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= PlayerConnectionHandler;
            NetworkManager.Singleton.OnClientDisconnectCallback -= PlayerDisconnectionHandler;
        }
    }
}
