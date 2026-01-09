using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider EaseHealthSlider;
    public float maxHealth = 100f;
    public float Health;
    private float lerpSpeed = 0.05f;

    void Start()
    {
        Health = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = Health;
        }

        if (EaseHealthSlider != null)
        {
            EaseHealthSlider.maxValue = maxHealth;
            EaseHealthSlider.value = Health;
        }
    }

    void Update()
    {
        if (healthSlider != null)
        {
            if (!Mathf.Approximately(healthSlider.value, Health))
                healthSlider.value = Health;
        }

        if (EaseHealthSlider != null && healthSlider != null)
        {
            float target = healthSlider.value;
            if (!Mathf.Approximately(EaseHealthSlider.value, target))
            {
                float t = lerpSpeed * Time.deltaTime * 60f;
                EaseHealthSlider.value = Mathf.Lerp(EaseHealthSlider.value, target, t);

                if (Mathf.Abs(EaseHealthSlider.value - target) < 0.0005f)
                    EaseHealthSlider.value = target;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0f, maxHealth);

        if (Health <= 0f)
        {
            // Call GameManager to handle game over
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            else
            {
                // Fallback: just pause the game
                Time.timeScale = 0;
                Debug.LogWarning("Game Over: No GameManager found.");
            }
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, maxHealth);
    }

    // ADD THIS METHOD TO FIX THE ERROR
    public void ResetHealth()
    {
        Health = maxHealth;
        
        if (healthSlider != null)
            healthSlider.value = Health;
            
        if (EaseHealthSlider != null)
            EaseHealthSlider.value = Health;
            
        Debug.Log($"Health reset to {maxHealth}");
    }
    
    // Optional: Add a method to check current health
    public float GetCurrentHealth()
    {
        return Health;
    }
}