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
                GameObject particle = SpawnParticle(particleHitGroundPrefab, other.ClosestPoint(transform.position));
                Destroy(particle, 2f);
            }

            //Debug.Log("Enemy hit by bullet!");
        }
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("HIT GROUND");

            GameObject particle = SpawnParticle(particleHitGroundPrefab, other.ClosestPoint(transform.position));
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
