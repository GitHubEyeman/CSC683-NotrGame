using UnityEngine;
using System.Collections;

public class PlayerCollisionDetect : MonoBehaviour
{
    public PlayerHPLink playerHPLink;
    public int obstacleDamage = 10;
    public float invincibilityTime = 1f;
    
    public Renderer playerRenderer;
    public Material originalMaterial;
    public Material damageMaterial;
    
    private bool isInvincible = false;
    private Coroutine blinkCoroutine;
    
    void Start()
    {
        if (playerHPLink == null)
            playerHPLink = GetComponent<PlayerHPLink>();
            
        if (playerRenderer != null && originalMaterial == null)
            originalMaterial = playerRenderer.material;
            
        Debug.Log("PlayerCollisionDetect initialized");
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name + " | Tag: " + other.tag);
        
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("HIT OBSTACLE! Applying damage: " + obstacleDamage);
            TakeDamage(obstacleDamage);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            Debug.Log("Hit by enemy bullet!");
            TakeDamage(10); // Or get damage from bullet script
            Destroy(other.gameObject);
        }
    }
    
    // Public method that can be called from anywhere (including enemy bullets)
    public void TakeDamage(int damageAmount)
    {
        if (isInvincible) 
        {
            Debug.Log("Player is invincible, skipping damage");
            return;
        }
        
        Debug.Log("Player takes " + damageAmount + " damage!");
        
        // Apply damage to health
        if (playerHPLink != null)
        {
            Debug.Log("Calling playerHPLink.TakeDamage(" + damageAmount + ")");
            playerHPLink.TakeDamage(damageAmount);
        }
        else
        {
            Debug.LogError("playerHPLink is NULL!");
        }
        
        // Start invincibility and visual effect
        StartCoroutine(InvincibilityAndBlink());
    }
    
    IEnumerator InvincibilityAndBlink()
    {
        isInvincible = true;
        Debug.Log("Starting invincibility period");
        
        float elapsedTime = 0f;
        float blinkInterval = 0.1f;
        
        while (elapsedTime < invincibilityTime)
        {
            if (playerRenderer != null && damageMaterial != null && originalMaterial != null)
            {
                // Toggle between materials
                playerRenderer.material = (playerRenderer.material == originalMaterial) 
                    ? damageMaterial : originalMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer or materials are not properly assigned!");
            }
            
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        
        // Restore original material
        if (playerRenderer != null && originalMaterial != null)
        {
            playerRenderer.material = originalMaterial;
        }
        
        isInvincible = false;
        Debug.Log("Invincibility ended");
    }
}