using UnityEngine;

public class CollectiblePickUp : MonoBehaviour
{
    [Header("Healing Settings")]
    public float healAmount = 20f;
    
    [Header("Score Settings")]
    public int scoreValue = 100; // Points when collected
    
    [Header("Visual Effects")]
    public ParticleSystem collectEffect;
    public AudioClip collectSound;
    
    void Start()
    {
        // Make sure we have a collider set as trigger
        Collider collider = GetComponent<Collider>();
        if (collider != null && !collider.isTrigger)
        {
            Debug.LogWarning("Collectible collider should be set as Trigger!");
            collider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Heal the player
            HealthBar healthBar = FindFirstObjectByType<HealthBar>();
            if (healthBar != null)
            {
                healthBar.Heal(healAmount);
            }

            // Add score
            Score scoreScript = FindFirstObjectByType<Score>();
            if (scoreScript != null)
            {
                scoreScript.score += scoreValue;
                Debug.Log($"Collected! +{scoreValue} points. Total: {scoreScript.score}");
            }
            
            // Play visual effect
            if (collectEffect != null)
            {
                ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, 2f);
            }
            
            // Play sound (if you have audio system)
            // if (collectSound != null)
            // {
            //     AudioSource.PlayClipAtPoint(collectSound, transform.position);
            // }

            Destroy(gameObject);
        }
    }
}