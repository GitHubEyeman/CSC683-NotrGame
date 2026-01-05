using UnityEngine;

public class EnemyBulletBehaviourScript : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;

    public ParticleSystem particlePrefab;

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
            GameObject particle = SpawnParticle(particlePrefab, other.ClosestPoint(transform.position));
            Destroy(particle, 2f);
            Destroy(gameObject);
        }
    }

    public GameObject SpawnParticle(ParticleSystem particlePrefab, Vector3 position)
    {
        // Instantiate the particle system at the given position
        ParticleSystem newParticle = Instantiate(particlePrefab, position, Quaternion.identity);

        // Play the particle system
        newParticle.Play();
        return newParticle.gameObject;
    }
}