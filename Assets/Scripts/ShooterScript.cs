using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bullet;
    public ParticleSystem muzzleFlash;
    public ParticleSystem hitEffect;
    
    [Header("Shooting Settings")]
    public float bulletSpeed = 30f;
    public float maxDistance = 100f;
    public float fireRate = 0.2f;
    public float damage = 10f;
    
    private float nextFireTime = 0f;
    private Camera mainCamera;
    
    void Start()
    {
        mainCamera = Camera.main;
        this.enabled = false; // Start disabled
    }

    void Update()
    {
        // Only shoot if game is running and not paused
        if (GameManager.Instance == null || !GameManager.Instance.isGameRunning || GameManager.Instance.isGamePaused)
            return;
            
        // Check for firing input
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    void Shoot()
    {
        // Play muzzle flash
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        // Create bullet
        if (bullet != null)
        {
            GameObject bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
            
            // Determine shoot direction (center of screen)
            Vector3 shootDirection = GetShootDirection();
            
            // Set bullet rotation to face direction
            if (shootDirection != Vector3.zero)
            {
                bulletObj.transform.rotation = Quaternion.LookRotation(shootDirection);
            }
            
            // Add velocity to bullet
            Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = shootDirection * bulletSpeed;
            }
            
            // Add bullet script to handle hits
            BulletScript bulletScript = bulletObj.GetComponent<BulletScript>();
            if (bulletScript == null)
            {
                bulletScript = bulletObj.AddComponent<BulletScript>();
            }
            bulletScript.damage = damage;
            
            // Destroy bullet after time
            Destroy(bulletObj, 3f);
        }
        
        // Check for hits
        CheckHit();
    }
    
    Vector3 GetShootDirection()
    {
        // Shoot from camera center (locked cursor)
        if (mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            return ray.direction;
        }
        
        // Fallback: forward from this object
        return transform.forward;
    }
    
    void CheckHit()
    {
        Ray ray = new Ray(transform.position, GetShootDirection());
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if hit enemy
            if (hit.collider.CompareTag("Enemy"))
            {
                // Try different ways to apply damage
                EnemyHpScript enemyHealth = hit.collider.GetComponent<EnemyHpScript>();
                if (enemyHealth != null)
                {
                    // Check if TakeDamage method exists
                    System.Reflection.MethodInfo method = enemyHealth.GetType().GetMethod("TakeDamage");
                    if (method != null)
                    {
                        method.Invoke(enemyHealth, new object[] { damage });
                    }
                    else
                    {
                        // Fallback: directly reduce health if property exists
                        System.Reflection.PropertyInfo healthProp = enemyHealth.GetType().GetProperty("currentHealth");
                        if (healthProp != null)
                        {
                            float current = (float)healthProp.GetValue(enemyHealth);
                            healthProp.SetValue(enemyHealth, current - damage);
                        }
                    }
                }
                
                // Spawn hit effect
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
            }
        }
    }
}