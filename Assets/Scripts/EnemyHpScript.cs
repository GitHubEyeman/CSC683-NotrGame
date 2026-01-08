using UnityEngine;

public class EnemyHpScript : MonoBehaviour
{
    public int hp = 3;
    public ParticleSystem particleDeadPrefab;
    
    // This will be called when the enemy is spawned
    public void InitializeHealth(int baseHealth)
    {
        hp = baseHealth;
    }

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
        ParticleSystem newParticle = Instantiate(particlePrefab, position, Quaternion.identity);
        newParticle.Play();
        return newParticle.gameObject;
    }
}