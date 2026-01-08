using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider EaseHealthSlider;
    public float maxHealth = 100f;
    public float Health;
    private float lerpSpeed = 0.05f;

    /* Pseudocode / Plan (detailed)
     - On Start: set current Health to maxHealth.
     - Each frame (Update):
       1. If `healthSlider` exists and its value differs from `Health`, set `healthSlider.value` directly to `Health`.
       2. If `EaseHealthSlider` and `healthSlider` both exist:
          a. Use the `healthSlider.value` as the target value to ease toward (keeps UI-driven value as source).
          b. If the eased slider is not approximately equal to the target:
             - Smoothly interpolate the eased slider's `value` toward the target using `Mathf.Lerp`.
             - Scale the lerp factor by `Time.deltaTime` so the easing is frame-rate independent.
             - After lerping, if the eased value is very close to the target, snap it to the exact target to avoid asymptotic never-reaching behavior.
     - Keep other methods (TakeDamage/Heal) the same but ensure Health stays clamped.
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame  
    void Update()
    {
        if (healthSlider != null)
        {
            if (!Mathf.Approximately(healthSlider.value, Health))
                healthSlider.value = Health;
        }

        if (EaseHealthSlider != null && healthSlider != null)
        {
            float target = healthSlider.value; // ease toward the visible health slider value
            if (!Mathf.Approximately(EaseHealthSlider.value, target))
            {
                // Scale lerp by Time.deltaTime for consistent easing across frame rates
                // Multiply by a constant to make lerpSpeed feel like the original value (tweak as needed)
                float t = lerpSpeed * Time.deltaTime * 60f;
                EaseHealthSlider.value = Mathf.Lerp(EaseHealthSlider.value, target, t);

                // Snap when very close to avoid never reaching the exact value
                if (Mathf.Abs(EaseHealthSlider.value - target) < 0.05f)
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
            Debug.Log("Player is Dead");
        }
            
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, maxHealth);
    }

}
