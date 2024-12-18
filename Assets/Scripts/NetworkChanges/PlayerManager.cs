using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame 
    {
        get
        {
            return playersInGame.Value;
        }    
    }


    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if(IsHost)
        {
            Debug.Log($"{clientId} just connected...");
            playersInGame.Value++;
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if(IsHost)
        {
            Debug.Log($"{clientId} just disconnected...");
            playersInGame.Value--;
        }
    }
}