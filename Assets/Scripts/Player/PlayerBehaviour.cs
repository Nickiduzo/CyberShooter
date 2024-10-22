using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [HideInInspector] public PlayerState currentState;
    
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] swords;
    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject rifle;

    private void Start()
    {
        SetPlayerState(PlayerState.Empty);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetPlayerState(PlayerState.Melee);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetPlayerState(PlayerState.Firegun);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetPlayerState(PlayerState.Empty);
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
            case PlayerState.Melee:
                animator.SetLayerWeight(2,1);
                SwitchSwords(true);
                break;
            case PlayerState.Firegun:
                animator.SetLayerWeight(1, 1);
                rifle.SetActive(true);
                break;
            case PlayerState.Empty:
                animator.SetLayerWeight(0, 1);
                SwitchOffAll();
                break;
        }
    }

    private void SwitchOffAll()
    {
        SwitchSwords(false);
        rifle.SetActive(false);
        //pistol.SetActive(false);
    }
    private void SwitchSwords(bool activate)
    {
        for(int i = 0; i < swords.Length;i++)
        {
            swords[i].SetActive(activate);
        }
    }
}
