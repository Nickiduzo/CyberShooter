using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public PlayerState currentState;
    
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] swords;
    [SerializeField] private GameObject sword;

    private void Awake()
    {
        EmptyHandler();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerState(PlayerState.Empty);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerState(PlayerState.TwoSwords);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
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
        animator.SetLayerWeight(3, 0);

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
        }
    }

    private void EmptyHandler()
    {
        currentState = PlayerState.Empty;

        animator.SetLayerWeight(1, 1);

        animator.SetLayerWeight(0, 0);
        animator.SetLayerWeight(2, 0);
        animator.SetLayerWeight(3, 0);
    }

    private void SwordHandler()
    {
        animator.SetLayerWeight(3, 1);
    }

    private void SwordsHandler()
    {
        animator.SetLayerWeight(2, 1);
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
