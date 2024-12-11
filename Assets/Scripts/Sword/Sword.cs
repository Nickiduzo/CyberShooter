using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    public UnityEvent<int> OnHit;

    public bool canDamage = false;

    public int swordDamage = 25;

    [SerializeField] private PlayerAnimation playerAnimation;
    private void Start()
    {
        playerAnimation.ActivateAttack.AddListener(SwordOn);
        playerAnimation.DeactivateAttack.AddListener(SwordOff);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(canDamage)
        {
            if(other.gameObject.CompareTag("Enemy"))
            {
                AudioManager.instanse.Play("HitOnEnemy");
                OnHit.Invoke(swordDamage);
            }
        }
    }

    private void SwordOn() => canDamage = true;
    private void SwordOff() => canDamage = false;
}
