using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerBehaviour))]
public class PlayerAttackTimer : NetworkBehaviour
{
    [SerializeField] private PlayerBehaviour playerBehaviour;

    [SerializeField] private GameObject cooldowns;
    [SerializeField] private Image usualImage;
    [SerializeField] private Image unusualImage;
    [SerializeField] private Image hardImage;
    [SerializeField] private Image ultimateImage;

    [Header("Two swords")]
    [SerializeField][Range(0,2f)] private float swordsAttack;
    [SerializeField][Range(0,2f)] private float swordsDoubleAttack;
    [SerializeField][Range(0,3.5f)] private float swordsChargeAttack;
    [SerializeField][Range(0,3.5f)] private float swordsDChargeAttack;

    [Header("Two Fast Swords")]
    [SerializeField][Range(0, 3f)] private float fastAttack;
    [SerializeField][Range(0, 3f)] private float doubleAttack;
    [SerializeField][Range(0, 3f)] private float chargeAttack;
    [SerializeField][Range(0, 3f)] private float dChargeAttack;

    [Header("One sword")]
    [SerializeField][Range(0, 7.5f)] private float swordAttack;
    [SerializeField][Range(0, 7.5f)] private float swordHard;
    [SerializeField][Range(0, 10f)] private float swordChargeAttack;
    [SerializeField][Range(0, 10f)] private float swordJumpAttack;

    private float currentTimer;

    private int currentImageIndex;
    private float maxCooldown;

    private PlayerState playerState;

    public bool isKick = false;
    private void Start()
    {
        SetState();
        InitializationCooldowns();
    }

    private void Update()
    {
        if (!IsOwner) return;

        SetState();
        
        if(!isKick)
        {
            HandlerAttack();
        }

        if(currentTimer <= 0)
        {
            isKick = false;
        }
        else
        {
            currentTimer -= Time.deltaTime;
        }

        UpdateCooldownUI();
    }

    private void HandlerAttack()
    {
        if (isKick) return;
        if(playerState == PlayerState.TwoSwords)
        {
            if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
            {
                SetCooldown(swordsChargeAttack, 3);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1))
            {
                SetCooldown(swordsDChargeAttack, 4);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                SetCooldown(swordsAttack, 1);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                SetCooldown(swordsDoubleAttack, 2);
            }
        }
    
        if(playerState == PlayerState.Sword)
        {
            if(Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
            {
                SetCooldown(swordChargeAttack, 3);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1))
            {
                SetCooldown(swordJumpAttack, 4);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                SetCooldown(swordAttack, 1);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                SetCooldown(swordHard, 2);
            }
        }

        if(playerState == PlayerState.FastSwords)
        {
            if(Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(0))
            {
                SetCooldown(chargeAttack, 3);
            }
            else if (Input.GetKey(KeyCode.W) && Input.GetMouseButtonDown(1))
            {
                SetCooldown(dChargeAttack, 4);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                SetCooldown(fastAttack, 1);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                SetCooldown(doubleAttack, 2);
            }
        }
    }

    private void SetCooldown(float cooldown, int index)
    {
        currentTimer = cooldown;
        maxCooldown = cooldown;

        isKick = true;

        currentImageIndex = index;
    }

    private void UpdateCooldownUI()
    {
        if (maxCooldown <= 0) return;
        switch (currentImageIndex)
        {
            case 1:
                usualImage.fillAmount = Mathf.Clamp01(1 - (currentTimer / maxCooldown));
                break;
            case 2:
                unusualImage.fillAmount = Mathf.Clamp01(1 - (currentTimer / maxCooldown));
                break;
            case 3:
                hardImage.fillAmount = Mathf.Clamp01(1 - (currentTimer / maxCooldown));
                break;
            case 4:
                ultimateImage.fillAmount = Mathf.Clamp01(1 - (currentTimer / maxCooldown));
                break;
        }
    }

    public bool GetKick()
    {
        return isKick;
    }

    private void SetState() => playerState = playerBehaviour.currentState;

    private void InitializationCooldowns()
    {
        cooldowns.SetActive(IsOwner);
    }
}
