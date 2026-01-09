using UnityEngine;

public class CollectiblePickUp : MonoBehaviour
{
    public float healAmount = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HealthBar healthBar = FindFirstObjectByType<HealthBar>();

            if (healthBar != null)
            {
                healthBar.Heal(healAmount);
            }

            Destroy(gameObject);
        }
    }
}
