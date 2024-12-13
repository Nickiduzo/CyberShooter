using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public PlayerState currentState;
    
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject[] fastSwords;

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
        
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerState(PlayerState.TwoSwords);
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerState(PlayerState.FastSwords);
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
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
}
