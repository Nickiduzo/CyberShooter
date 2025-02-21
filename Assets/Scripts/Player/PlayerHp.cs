using Unity.Netcode;
using UnityEngine;

public class PlayerHp : NetworkBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SkinnedMeshRenderer skin;
    
    private int hp;

    private NetworkVariable<int> materialIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);   

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            hp = 100;
            SetRandomMaterial();
        }

        materialIndex.OnValueChanged += (oldValue, newValue) =>
        {
            ApplyMaterial(newValue);
        };

        ApplyMaterial(materialIndex.Value);
    }

    private void SetRandomMaterial()
    {
        int rand = Random.Range(0, 3);
        print(rand);
        ChangeSkinServerRpc(rand);
    }

    private void ApplyMaterial(int index)
    {
        switch (index)
        {
            case 0:
                SetYellow();
                break;
            case 1:
                SetRed();
                break;
            case 2:
                SetGreen();
                break;
            default:
                SetStandard();
                break;
        }
    }

    private void SetStandard()
    {
        skin.materials = new Material[] { playerData.body, playerData.cables, playerData.head, playerData.ribs};
    }

    private void SetYellow()
    {
        skin.materials = new Material[] { playerData.bodyYellow, playerData.cablesYellow, playerData.headYellow, playerData.ribsYellow };
    }

    private void SetRed()
    {
        skin.materials = new Material[] { playerData.bodyRed, playerData.cablesRed, playerData.headRed, playerData.ribsRed };
    }

    private void SetGreen()
    {
        skin.materials = new Material[] { playerData.bodyGreen, playerData.cablesGreen, playerData.headGreen, playerData.ribsGreen };
    }

    [ServerRpc]
    private void ChangeSkinServerRpc(int index)
    {
        materialIndex.Value = index;
    }
}
