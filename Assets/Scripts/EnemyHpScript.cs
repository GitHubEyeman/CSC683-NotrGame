using UnityEngine;

public class EnemyHpScript : MonoBehaviour
{
    
    public int hp = 3;
    public ParticleSystem particleDeadPrefab;

    public void takeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            
            GameObject particle = SpawnParticle(particleDeadPrefab, transform.position);

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
