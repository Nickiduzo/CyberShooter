using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : NetworkBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SkinnedMeshRenderer skin;

    [SerializeField] private Slider hpSlider;

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private TextMeshProUGUI amountOfHealth;
    [SerializeField] private GameObject[] playerElements;

    [SerializeField] private GameObject over;

    private NetworkVariable<int> materialIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);   

    private NetworkVariable<int> healthPoints = new NetworkVariable<int>(1000,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server);

    private void Start()
    {
        SliderInitialization();
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            ApplyMaterial();
            SetHealth(1000);
            RequestSyncServerRpc(playerData.currentColor);
        }
        else
        {
            ApplyMaterialByIndex(materialIndex.Value);
        }

        materialIndex.OnValueChanged += (oldIndex, newIndex) =>
        {
            ApplyMaterialByIndex(newIndex);
        };
    }

    private void ApplyMaterial()
    {
        ApplyMaterialByIndex(playerData.currentColor);
    }

    private void ApplyMaterialByIndex(int index)
    {
        switch(index)
        {
            case 0: SetStandard(); break;
            case 1: SetGreen(); break;
            case 2: SetYellow(); break;
            case 4: SetRed(); break;
        }
    }

    private void SetHealth(int value)
    {
        healthPoints.Value = value;
        hpSlider.value = healthPoints.Value;
        amountOfHealth.text = value.ToString();
    }

    [ServerRpc]
    private void RequestSyncServerRpc(int colorIndex, ServerRpcParams rpcParams = default)
    {
        materialIndex.Value = colorIndex;   
        ApplyMaterialClientRpc(colorIndex);
    }

    [ClientRpc]
    private void ApplyMaterialClientRpc(int index)
    {
        ApplyMaterialByIndex(index);
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

    public void TakeDamage(int damage)
    {
        DecreaseHpServerRpc(damage);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DecreaseHpServerRpc(int damage)
    {
        if(healthPoints.Value - damage > 0)
        {
            healthPoints.Value -= damage;
            UpdateClientRpc(healthPoints.Value);
        }
        else
        {
            DieServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DieServerRpc()
    {
        ShowOverMessageClientRpc();

        healthPoints.Value = 0;
        player.SetActive(false);

        foreach (var item in playerElements)
        {
            item.SetActive(false);
        }

        RespawnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RespawnServerRpc()
    {
        healthPoints.Value = 1000;
        RespawnClientRpc();

        foreach (var item in playerElements)
        {
            item.SetActive(true);
        }

        player.SetActive(true);
        UpdateClientRpc(healthPoints.Value);
    }

    [ClientRpc]
    private void RespawnClientRpc()
    {
        if (IsOwner) 
        {
            playerMove.Spawn();
            playerBehaviour.RespawnPlayerBehaviour();
        }
    }

    [ClientRpc]
    private void ShowOverMessageClientRpc()
    {
        if (IsOwner)
        {
            over.SetActive(true);
        }
    }

    [ClientRpc]
    private void UpdateClientRpc(int newHp)
    {
        hpSlider.value = newHp;
        amountOfHealth.text = newHp.ToString();
    }

    private void SliderInitialization()
    {
        if(IsOwner)
        {
            hpSlider.gameObject.SetActive(true);
        }
        else
        {
            hpSlider.gameObject.SetActive(false);
        }
    }
}
