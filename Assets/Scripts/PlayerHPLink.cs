using UnityEngine;

public class PlayerHPLink : MonoBehaviour
{
    public HealthBar healthBar;

    
    public void TakeDamage(float damage)
    {
        Debug.Log("PEPEPE");
        healthBar.TakeDamage(damage);
    }
}
