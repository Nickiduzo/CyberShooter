using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerBehaviour : NetworkBehaviour
{
    [HideInInspector] public PlayerState currentState;

    [HideInInspector] public NetworkVariable<PlayerState> networkState = new NetworkVariable<PlayerState>();

    [SerializeField] private Animator animator;

    private GameObject[] fastSwords;
    [SerializeField] private SwordsEquipment[] fastSwordsEquipment;
    private NetworkVariable<int> selectedFastSwordsIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private GameObject[] swords;
    [SerializeField] private SwordsEquipment[] swordsEquipment;
    private NetworkVariable<int> selectedSwordsIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    private GameObject sword;
    [SerializeField] private SwordsEquipment[] swordEquipment;
    private NetworkVariable<int> selectedSwordIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        EmptyHandler();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        selectedSwordsIndex.OnValueChanged += (oldValue, newValue) => EquipSwords(newValue);
        selectedFastSwordsIndex.OnValueChanged += (oldValue, newValue) => EquipFastSwords(newValue);
        selectedSwordIndex.OnValueChanged += (oldValue, newValue) => EquipHardSword(newValue);

        if (IsOwner)
        {
            // ѕримен€ем сохраненный выбор меча
            SelectSwords(playerData.currentSwords);
            SelectFastSwords(playerData.currentFastSwords);
            SelectHardSword(playerData.currentSword);
        }
        else
        {
            // ƒл€ других игроков просто примен€ем текущий выбор меча
            EquipSwords(selectedSwordsIndex.Value);
            EquipFastSwords(selectedFastSwordsIndex.Value);
            EquipHardSword(selectedSwordIndex.Value);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerStateServerRpc(PlayerState.Empty);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerStateServerRpc(PlayerState.TwoSwords);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerStateServerRpc(PlayerState.FastSwords);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetPlayerStateServerRpc(PlayerState.Sword);
        }
    }

    [ServerRpc]
    private void SetPlayerStateServerRpc(PlayerState newState)
    {
        networkState.Value = newState;
        SetPlayerStateClientRpc(newState);
    }

    [ClientRpc]
    private void SetPlayerStateClientRpc(PlayerState newState) => SetPlayerState(newState);

    public void RespawnPlayerBehaviour()
    {
        if(IsOwner)
        {
            playerStats.AddDeathServerRpc();
            SetPlayerStateServerRpc(PlayerState.Empty);
        }
    }

    private void SetPlayerState(PlayerState newState)
    {
        currentState = newState;

        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(3, 0);
        animator.SetLayerWeight(4, 0);

        SwitchOffAll();

        switch (newState)
        {
            case PlayerState.Empty:
                EmptyHandler();
                SwitchOffAll();
                break;
            case PlayerState.TwoSwords:
                SwordsHandler();
                SwitchSwords(true);
                break;
            case PlayerState.Sword:
                SwordHandler();
                SwitchSword(true);
                break;
            case PlayerState.FastSwords:
                FastSwordsHandler();
                SwitchFastSwords(true);
                break;
        }
    }

    private void EmptyHandler()
    {
        currentState = PlayerState.Empty;

        animator.SetLayerWeight(1, 1);

        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(3, 0);
        animator.SetLayerWeight(4, 0);
    }

    private void SwordHandler()
    {
        animator.SetLayerWeight(3, 1);
    }

    private void SwordsHandler()
    {
        animator.SetLayerWeight(2, 1);
    }

    private void FastSwordsHandler()
    {
        animator.SetLayerWeight(4, 1);
    }

    private void SwitchOffAll()
    {
        SwitchSwords(false);
        SwitchSword(false);
        SwitchFastSwords(false);
    }

    private void SwitchSword(bool activate)
    {
        sword.gameObject.SetActive(activate);
    }

    private void SwitchSwords(bool activate)
    {
        for(int i = 0; i < swords.Length;i++)
        {
            swords[i].SetActive(activate);
        }
    }

    private void SwitchFastSwords(bool activate)
    {
        for(int i = 0; i < fastSwords.Length;i++)
            fastSwords[i].SetActive(activate);
    }

    private void SelectSwords(int newIndex)
    {
        if (!IsOwner) return;

        if(newIndex >= 0 && newIndex < swordsEquipment.Length)
        {
            playerData.currentSwords = newIndex;
            selectedSwordsIndex.Value = newIndex;
        }
    }

    private void EquipSwords(int index)
    {
        if(index >= 0 && index < swordsEquipment.Length)
        {
            swords = swordsEquipment[index].leftAndRightSwords;
        }
    }

    private void SelectFastSwords(int newIndex)
    {
        if (!IsOwner) return;

        if(newIndex >= 0 && newIndex < fastSwordsEquipment.Length)
        {
            playerData.currentFastSwords = newIndex;
            selectedFastSwordsIndex.Value = newIndex;
        }
    }

    private void EquipFastSwords(int index)
    {
        if(index >= 0 && index < fastSwordsEquipment.Length)
        {
            fastSwords = fastSwordsEquipment[index].leftAndRightSwords;
        }
    }

    private void SelectHardSword(int newIndex)
    {
        if (!IsOwner) return;

        if(newIndex >= 0 && newIndex < swordEquipment.Length)
        {
            playerData.currentSword = newIndex;
            selectedSwordIndex.Value = newIndex;
        }
    }

    private void EquipHardSword(int index)
    {
        if(index >= 0 && index < swordEquipment.Length)
        {
            sword = swordEquipment[index].leftAndRightSwords[0];
        }
    }
}

[Serializable]
public class SwordsEquipment
{
    public int swordsIndex;
    public GameObject[] leftAndRightSwords;
}
