using UnityEngine;

public class ObstacleDestroyableScript : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    private int currentHP;
    public ParticleSystem particlePrefab;

    private void Start()
    {
        currentHP = maxHP;
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
        {
            SpawnParticle(particlePrefab, transform.position);
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
