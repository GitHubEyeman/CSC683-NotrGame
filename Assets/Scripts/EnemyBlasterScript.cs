using System.Collections;
using UnityEngine;

public class EnemyBlasterScript : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public ParticleSystem particlePrefab;

    public float chargeTime = 3f;
    public float bulletSpeed = 50f;

    private float chargeTimer = -1f;
    private bool isCharging = false;

    void Update()
    {
        if (player == null) return;

        FacePlayer();
        ChargeAndShoot();
    }

    void ChargeAndShoot()
    {
        chargeTimer += Time.deltaTime;

        // Optional: visual feedback while charging
        // Debug.Log("Charging...");
        
        if (!isCharging) {
            SpawnParticle(particlePrefab, firePoint.position);
            isCharging = true;
        }

        if (chargeTimer >= chargeTime)
        {
            Shoot();
            chargeTimer = 0f;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector3 direction = (player.position - firePoint.position).normalized;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed;

        isCharging = false;
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }

    IEnumerator HandleTrigger(Collider other)
    {
        // Spawn particle
        Instantiate(particlePrefab, other.transform.position, Quaternion.identity);

        // Wait 1 second
        yield return new WaitForSeconds(chargeTime - 0.5f);

        // Code that runs AFTER waiting
        Debug.Log("One second has passed!");
    }

    public void SpawnParticle(ParticleSystem particlePrefab, Vector3 position)
    {
        // Instantiate the particle system at the given position
        ParticleSystem newParticle = Instantiate(particlePrefab, position, Quaternion.identity, firePoint.transform);
        
        // Play the particle system
        newParticle.Play();
        Destroy(newParticle.gameObject, chargeTime - 0.5f);
    }
}
