using UnityEngine;

public class PlayerHPLink : MonoBehaviour
{
    public HealthBar healthBar;
    
    public void TakeDamage(float damage)
    {
        Debug.Log("PlayerHPLink.TakeDamage called with: " + damage);
        
        if (healthBar != null)
        {
            healthBar.TakeDamage(damage);
            Debug.Log("Damage passed to HealthBar");
        }
        else
        {
            Debug.LogError("HealthBar is not assigned in PlayerHPLink!");
        }
    }
}