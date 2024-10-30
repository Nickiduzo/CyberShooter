using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    private int hp;

    [SerializeField] private int maxHp = 100;
    private readonly int minHp = 0;

    [SerializeField] private Sword leftSword;
    [SerializeField] private Sword rightSword;

    private void Start()
    {
        hp = maxHp;

        leftSword.OnHit.AddListener(DecreaseHp);
        rightSword.OnHit.AddListener(DecreaseHp);
    }

    public void DecreaseHp(int damage)
    {
        if(hp > minHp)
        {
            hp -= damage;
            print(hp + " total health");
        }
        else
        {
            print("Enemy die");
            Destroy(gameObject);
        }
    }
}
