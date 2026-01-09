/* Pseudocode / Plan (detailed)
 - On Start:
   1. Set current `Health` to `maxHealth`.
   2. If `healthSlider` exists:
      a. Set its `maxValue` to `maxHealth`.
      b. Set its `value` to `Health`.
   3. If `EaseHealthSlider` exists:
      a. Set its `maxValue` to `maxHealth`.
      b. Set its `value` to `Health`.
 - Each frame (Update):
   1. If `healthSlider` exists and `healthSlider.value` differs from `Health`, assign `healthSlider.value = Health`.
   2. If both `EaseHealthSlider` and `healthSlider` exist:
      a. Compute `target = healthSlider.value`.
      b. If `EaseHealthSlider.value` is not approximately equal to `target`:
         - Compute interpolation factor `t = lerpSpeed * Time.deltaTime`.
         - Update eased value: `EaseHealthSlider.value = Mathf.Lerp(EaseHealthSlider.value, target, t)`.
         - If very close to `target` (e.g. within 0.05), snap `EaseHealthSlider.value = target`.
 - TakeDamage(damage):
   1. Subtract `damage` from `Health`.
   2. Clamp `Health` between 0 and `maxHealth`.
   3. If `Health <= 0`:
      a. If `GameOverPanel` is not null, call `GameOverPanel.SetActive(true)` (call method, not assign).
      b. Pause game with `Time.timeScale = 0`.
 - Heal(amount):
   1. Add `amount` to `Health`.
   2. Clamp `Health` between 0 and `maxHealth`.
 - Always null-check GameObject/Sliders before accessing methods/properties.
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public GameObject GameOverPanel;
    public Slider healthSlider;
    public Slider EaseHealthSlider;
    public float maxHealth = 100f;
    public float Health;
    private float lerpSpeed = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            if (GameOverPanel != null)
            {
                // Correct usage: call the SetActive method (do not assign to it)
                GameOverPanel.SetActive(true);
            }
            Time.timeScale = 0;
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        Health = Mathf.Clamp(Health, 0f, maxHealth);
    }
}
