using UnityEngine;

public class EnemyHpScript : MonoBehaviour
{
    
    public int hp = 3;
    public ParticleSystem particleDeadPrefab;
    //public AudioSource enemyDestroy;

    public void takeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            
            GameObject particle = SpawnParticle(particleDeadPrefab, transform.position);
            Score score = FindFirstObjectByType<Score>();
            score.score += 1000;

            Destroy(particle, 2f);

            //EnemyDeath
            
            AudioManager.Instance.Play("EnemyDeath");
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
