using UnityEngine;

public class EnemyBlasterScript : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public float chargeTime = 2f;
    public float bulletSpeed = 50f;

    private float chargeTimer = -1f;

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
    }

    void FacePlayer()
    {
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookDir);
    }
}
