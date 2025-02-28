using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    private Dictionary<ulong, PlayerStats> players = new Dictionary<ulong, PlayerStats>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        NetworkManager.Singleton.OnClientConnectedCallback += PlayerConnectionHandler;
        NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnectionHandler;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            ulong hostId = NetworkManager.Singleton.LocalClientId;
            PlayerStats hostPlayer = NetworkManager.Singleton.ConnectedClients[hostId].PlayerObject.GetComponent<PlayerStats>();
            if(hostPlayer != null)
            {
                players[hostId] = hostPlayer;
            }
        }
    }

    private void PlayerConnectionHandler(ulong playerId)
    {
        if (!IsOwner) return;
        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(playerId)) return;

        PlayerStats player = NetworkManager.Singleton.ConnectedClients[playerId].PlayerObject.GetComponent<PlayerStats>();
        if (player != null)
        {
            players[playerId] = player;
        }
    }

    private void PlayerDisconnectionHandler(ulong playerId)
    {
        if (players.ContainsKey(playerId))
        {
            players.Remove(playerId);
        }
    }

    public List<PlayerStats> GetAllPlayers()
    {
        return new List<PlayerStats>(players.Values);
    }

    [ServerRpc(RequireOwnership = false)] // Клиенты теперь могут запрашивать игроков
    public void RequestPlayersServerRpc(ulong clientId)
    {
        List<ulong> playerIds = new List<ulong>(players.Keys);
        SendPlayersClientRpc(clientId, playerIds.ToArray());
    }

    [ClientRpc]
    private void SendPlayersClientRpc(ulong requestingClientId, ulong[] playerIds)
    {
        if (NetworkManager.Singleton.LocalClientId == requestingClientId)
        {
            players.Clear();
            foreach (ulong id in playerIds)
            {
                if (NetworkManager.Singleton.ConnectedClients.ContainsKey(id))
                {
                    PlayerStats player = NetworkManager.Singleton.ConnectedClients[id].PlayerObject.GetComponent<PlayerStats>();
                    if (player != null)
                    {
                        players[id] = player;
                    }
                }
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if(IsOwner)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= PlayerConnectionHandler;
            NetworkManager.Singleton.OnClientDisconnectCallback -= PlayerDisconnectionHandler;
        }
    }
}
