using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private PlayerData playerData;

    public NetworkVariable<int> Kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> Deaths = new NetworkVariable<int>(0);
    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>("Player");

    public float KD => Deaths.Value == 0 ? Kills.Value : (float)Kills.Value / Deaths.Value;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(IsOwner)
        {
            SetPlayerNameServerRpc(playerData.currentName);
        }

        gameObject.name = PlayerName.Value.ToString();

        PlayerName.OnValueChanged += (oldValue, newValue) =>
        {
            gameObject.name = newValue.ToString();
        };
    }

    [ServerRpc]
    private void SetPlayerNameServerRpc(string newName)
    {
        PlayerName.Value = new FixedString64Bytes(newName);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddKillServerRpc()
    {
        Kills.Value++;
    }

    [ServerRpc]
    public void AddDeathServerRpc()
    {
        Deaths.Value++;
    }
}
