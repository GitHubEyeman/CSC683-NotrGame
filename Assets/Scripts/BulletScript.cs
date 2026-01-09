using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 3f;
    
    void Start()
    {
        // Destroy bullet after lifetime
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Don't hit player or other bullets
        if (other.CompareTag("Player") || other.CompareTag("Bullet"))
            return;
        
        // Hit enemy
        if (other.CompareTag("Enemy"))
        {
            // Try to get EnemyHpScript
            EnemyHpScript enemyHealth = other.GetComponent<EnemyHpScript>();
            if (enemyHealth != null)
            {
                // Use reflection to call TakeDamage if it exists
                System.Reflection.MethodInfo method = enemyHealth.GetType().GetMethod("TakeDamage");
                if (method != null)
                {
                    method.Invoke(enemyHealth, new object[] { damage });
                }
            }
        }
        
        // Destroy bullet on hit
        Destroy(gameObject);
    }
}