using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{

    public int damage = 1;
    public ParticleSystem particleHitGroundPrefab;


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet Trigger entered with: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.gameObject.CompareTag("Enemy"))
        {
            
            Destroy(gameObject);  
            EnemyHpScript enemy = other.GetComponent<EnemyHpScript>();
            if (enemy != null) { 
                enemy.takeDamage(damage);
                SpawnParticle(particleHitGroundPrefab, other.ClosestPoint(transform.position));
            }

            //Debug.Log("Enemy hit by bullet!");
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("HIT GROUND");
            
            
            Destroy(gameObject);
        }

    }

    public void SpawnParticle(ParticleSystem particlePrefab, Vector3 position)
    {
        // Instantiate the particle system at the given position
        ParticleSystem newParticle = Instantiate(particlePrefab, position, Quaternion.identity);

        // Play the particle system
        newParticle.Play();
    }

}
