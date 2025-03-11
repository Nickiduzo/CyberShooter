using Unity.Netcode;
using UnityEngine;

public class PlayerBehaviour : NetworkBehaviour
{
    [HideInInspector] public PlayerState currentState;

    [HideInInspector] public NetworkVariable<PlayerState> networkState = new NetworkVariable<PlayerState>();

    [SerializeField] private Animator animator;

    private GameObject[] fastSwords;
    [SerializeField] private GameObject[] leftFastSwords;
    [SerializeField] private GameObject[] rightFastSwords;
    private NetworkVariable<int> selectedFastSwordsIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private GameObject[] swords;
    [SerializeField] private GameObject[] leftSwords;
    [SerializeField] private GameObject[] rightSwords;
    private NetworkVariable<int> selectedSwordsIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);
    
    private GameObject sword;
    [SerializeField] private GameObject[] hardSwords;
    private NetworkVariable<int> selectedSwordIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerAttackTimer playerAttackTimer;
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
            SelectSwords(playerData.currentSwords);
            SelectFastSwords(playerData.currentFastSwords);
            SelectHardSword(playerData.currentSword);
        }
        else
        {
            EquipSwords(selectedSwordsIndex.Value);
            EquipFastSwords(selectedFastSwordsIndex.Value);
            EquipHardSword(selectedSwordIndex.Value);
        }

        EquipSwords(selectedSwordsIndex.Value);
        EquipFastSwords(selectedFastSwordsIndex.Value);
        EquipHardSword(selectedSwordIndex.Value);
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (playerAttackTimer.isKick) return;

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

    public void RespawnPlayerBehaviour(PlayerState lastState)
    {
        if(IsOwner)
        {
            playerStats.AddDeathServerRpc();
            SetPlayerStateServerRpc(PlayerState.Empty);
        }
    }

    public void SetPlayerState(PlayerState newState)
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

    private void SwordsHandler()
    {
        animator.SetLayerWeight(2, 1);
    }

    private void SwordHandler()
    {
        animator.SetLayerWeight(3, 1);
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

        if(newIndex >= 0 && leftSwords.Length > 0 && rightSwords.Length > 0 &&
           newIndex < leftSwords.Length && newIndex < rightSwords.Length)
        {
            playerData.currentSwords = newIndex;
            selectedSwordsIndex.Value = newIndex;
        }
    }

    private void EquipSwords(int index)
    {
        if(index >= 0 && leftSwords.Length > 0 && rightSwords.Length > 0 &&
           index < leftSwords.Length && index < rightSwords.Length)
        {
            swords = new GameObject[] { leftSwords[index], rightSwords[index]};
        }
    }

    private void SelectFastSwords(int newIndex)
    {
        if (!IsOwner) return;

        if(newIndex >= 0 && leftFastSwords.Length > 0 && rightFastSwords.Length > 0 &&
           newIndex < leftFastSwords.Length && newIndex < rightFastSwords.Length)
        {
            playerData.currentFastSwords = newIndex;
            selectedFastSwordsIndex.Value = newIndex;
        }
    }

    private void EquipFastSwords(int index)
    {
        if(index >= 0 && leftFastSwords.Length > 0 && rightFastSwords.Length > 0 &&
           index < leftFastSwords.Length && index < rightFastSwords.Length)
        {
            fastSwords = new GameObject[] { leftFastSwords[index], rightFastSwords[index] };
        }
    }

    private void SelectHardSword(int newIndex)
    {
        if (!IsOwner) return;

        if(newIndex >= 0 && hardSwords.Length > 0 && newIndex < hardSwords.Length)
        {
            playerData.currentSword = newIndex;
            selectedSwordIndex.Value = newIndex;
        }
    }

    private void EquipHardSword(int index)
    {
        if(index >= 0 && hardSwords.Length > 0 && index < hardSwords.Length)
        {
            sword = hardSwords[index];
        }
    }
}

