using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : NetworkBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private SkinnedMeshRenderer skin;

    [SerializeField] private Slider hpSlider;

    [SerializeField] private GameObject player;
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
            SetRandomMaterial();

            healthPoints.Value = 1000;
            hpSlider.value = healthPoints.Value;
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
            print("Player die");
            player.SetActive(false);

            foreach (var item in playerElements)
            {
                item.SetActive(false);
            }

            over.SetActive(true);
        }
    }

    [ClientRpc]
    private void UpdateClientRpc(int newHp)
    {
        hpSlider.value = newHp;
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
