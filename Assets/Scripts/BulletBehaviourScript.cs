using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{

    public int damage = 1;

    

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(gameObject);  
            EnemyHpScript enemy = other.GetComponent<EnemyHpScript>();
            if (enemy != null) { 
                enemy.takeDamage(damage);
            }


            Debug.Log("Enemy hit by bullet!");
        }
    }

}
