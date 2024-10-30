using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public PlayerState currentState;
    
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] swords;
    [SerializeField] private GameObject sword;

    private void Awake()
    {
        SetPlayerState(PlayerState.Empty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerState(PlayerState.TwoSwords);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerState(PlayerState.Pistols);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerState(PlayerState.Empty);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetPlayerState(PlayerState.Sword);
        }
    }
    private void SetPlayerState(PlayerState newState)
    {
        currentState = newState;

        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(1, 0);
        animator.SetLayerWeight(2, 0);

        SwitchOffAll();

        switch (newState)
        {
            case PlayerState.TwoSwords:
                animator.SetLayerWeight(2,1);
                SwitchSwords(true);
                break;
            case PlayerState.Empty:
                animator.SetLayerWeight(0, 1);
                SwitchOffAll();
                break;
            case PlayerState.Sword:
                animator.SetLayerWeight(2, 1);
                SwitchSword(true);
                break;
        }
    }

    private void SwitchOffAll()
    {
        SwitchSwords(false);
        SwitchSword(false);
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
}
