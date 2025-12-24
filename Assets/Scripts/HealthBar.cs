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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Health = maxHealth;
    }

    // Update is called once per frame  
    void Update()
    {
        if (healthSlider != null && healthSlider.value != Health)
        {
            healthSlider.value = Health;
        }


        if (EaseHealthSlider != null && healthSlider != null && healthSlider.value != EaseHealthSlider.value)
        {
            EaseHealthSlider.value = Mathf.Lerp(EaseHealthSlider.value, Health, lerpSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Health = Mathf.Clamp(Health, 0f, maxHealth);
    }
}
