using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{

    public int damage = 1;

    

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet Trigger entered with: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(gameObject);  
            EnemyHpScript enemy = other.GetComponent<EnemyHpScript>();
            if (enemy != null) { 
                enemy.takeDamage(damage);
            }

            //Debug.Log("Enemy hit by bullet!");
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("HIT GROUND");
            Destroy(gameObject);
        }

    }

}
