using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private int hp;

    [SerializeField] private int maxHp = 1000;
    private int minHp = 0;

    [SerializeField] private Sword leftSword;
    [SerializeField] private Sword rightSword;
    private void Start()
    {
        hp = maxHp;
        leftSword.OnHit.AddListener(DecreaseHp);
        rightSword.OnHit.AddListener(DecreaseHp);
    }

    private void DecreaseHp(int damage)
    {
        if(hp < minHp)
        {
            print("Enemy died");
        }
        else
        {
            AudioManager.instanse.Play("HitOnEnemy");
            hp -= damage;
        }
    }
}
