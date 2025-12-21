using UnityEngine;

public class EnemyHpScript : MonoBehaviour
{
    
    public int hp = 3;


    public void takeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }


}
