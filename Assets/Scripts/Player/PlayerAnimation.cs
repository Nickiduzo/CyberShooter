using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    public UnityEvent ActivateAttack;
    public UnityEvent DeactivateAttack;

    [SerializeField] private Animator anim;
    [SerializeField] private PlayerBehaviour playerBehaviour;
    [SerializeField] private PlayerSwordAttack playerSwordAttack;

    private float x;
    private float y;

    private bool isGrounded;

    private float hitCooldown;
    private void Awake()
    {
        InitializeListeners();
        hitCooldown = 1.5f;
    }

    private void Update()
    {
        anim.SetBool("Grounded", isGrounded);
        Dance();
        StartMoving();
        MoveBehaviour();
        hitCooldown -= Time.deltaTime;
    }
    private void MoveBehaviour()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        float speedFactor = 0.1f;

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            speedFactor = 0.6f;
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            speedFactor = 1f;
        }

        anim.SetFloat("x", x * speedFactor);
        anim.SetFloat("y", y * speedFactor);
    }

    private void InitializeListeners()
    {
        playerSwordAttack.PlayAnimation.AddListener(DetectSwordAnimation);
    }
    private void Dance()
    {
        if (Input.GetKeyDown(KeyCode.G) && x == 0 && y == 0 && playerBehaviour.currentState == PlayerState.Empty)
        {
            anim.SetTrigger("Dance");
            AudioManager.instanse.Play("Polskaya");
        }
    }

    public void Jump() => anim.SetTrigger("isJump");
    public void DetectSwordAnimation(int animationIndex)
    {
        if (hitCooldown <= 0)
        {
            switch (animationIndex)
            {
                case 1:
                    SwordAttack();
                    hitCooldown = 1f;
                    break;
                case 2:
                    SwordDoubleAttack();
                    hitCooldown = 1.4f;
                    break;
                case 3:
                    SwordChargeAttack();
                    hitCooldown = 1.8f;
                    break;
                case 4:
                    SwordChargeDoubleAttack();
                    hitCooldown = 2f;
                    break;
                case 5:
                    SwordAttack();
                    hitCooldown = 2f;
                    break;
                case 6:
                    SwordDoubleAttack();
                    hitCooldown = 3f;
                    break;
                case 7:
                    SwordChargeAttack();
                    hitCooldown = 3f;
                    break;
                case 8:
                    SwordChargeDoubleAttack();
                    hitCooldown = 4f;
                    break;
            }
        }
    }
    private void SwordAttack()
    {
        anim.SetTrigger("isSword");
    }

    private void SwordDoubleAttack()
    {
        anim.SetTrigger("isSwordDouble");
    }

    private void SwordChargeAttack()
    {
        anim.SetTrigger("isSwordCharge");
    }

    private void SwordChargeDoubleAttack()
    {
        anim.SetTrigger("isSwordChargeDouble");
    }
    public void SwordAttackOn()
    {
        ActivateAttack?.Invoke();
    }

    public void SwordAttackOff()
    {
        DeactivateAttack?.Invoke();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") 
            || collision.gameObject.CompareTag("Building"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;  
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void StartMoving()
    {
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            anim.SetBool("isMove", true);
        }

        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            anim.SetBool("isMove", false);
        }
    }
}
