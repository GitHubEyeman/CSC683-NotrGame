using UnityEngine;

public class CollectiblePickUp : MonoBehaviour
{
    
    public float healAmount = 20f;
    public bool isBlasterUpgrade = false;
    public int blasterUpgradeType = 0;

    public ParticleSystem particlePrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthBar healthBar = FindFirstObjectByType<HealthBar>();
            ShooterScript shooterScript = FindFirstObjectByType<ShooterScript>();

            if (healthBar != null)
            {
                healthBar.Heal(healAmount);
            }

            if (isBlasterUpgrade) {
                shooterScript.upgradeBlaster(blasterUpgradeType);
            }



            if (particlePrefab != null) SpawnParticle(particlePrefab, transform.position);
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
