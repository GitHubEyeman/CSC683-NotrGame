using UnityEngine;

public class EnemyBulletBehaviourScript : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy bullet hit player!");
            
            // Try to find PlayerCollisionDetect on the player
            PlayerCollisionDetect playerCollision = other.GetComponent<PlayerCollisionDetect>();
            
            // If not found, try to find it in parent
            if (playerCollision == null && other.transform.parent != null)
            {
                playerCollision = other.transform.parent.GetComponent<PlayerCollisionDetect>();
            }
            
            // If found, use it for damage with blinking
            if (playerCollision != null)
            {
                playerCollision.TakeDamage(damage);
                Debug.Log("Damage applied with blinking effect!");
            }
            else
            {
                // Fallback: Direct damage without blinking
                PlayerHPLink playerHPLink = other.GetComponent<PlayerHPLink>();
                if (playerHPLink == null && other.transform.parent != null)
                {
                    playerHPLink = other.transform.parent.GetComponent<PlayerHPLink>();
                }
                
                if (playerHPLink != null)
                {
                    playerHPLink.TakeDamage(damage);
                    Debug.Log("Damage applied (no blinking - PlayerCollisionDetect not found)");
                }
            }
            
            Destroy(gameObject);
        }
    }
}