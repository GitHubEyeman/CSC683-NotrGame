using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bullet;
    public ParticleSystem particlePrefab;
    public float maxDistance = 50f;
    public float speed = 15f;
    public float spawnInterval = 0.3f;

    [Header("Gameplay State")]
    public bool canShoot = false; // Set by MainMenuManager

    private float timeSinceLastSpawn = 0f;

    void Start()
    {
        // Initially can't shoot (we're in menu)
        canShoot = false;
    }

    void Update()
    {
        // Don't process shooting if we can't shoot or game is paused
        if (!canShoot || Time.timeScale == 0f)
            return;

        timeSinceLastSpawn += Time.deltaTime;

        // Only allow shooting when mouse button is pressed AND enough time has passed
        if (Input.GetMouseButton(0) && timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if we hit something (and it's not the player)
        if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player"))
        {
            SpawnBullet(hit.point);
        }
        else
        {
            // If we don't hit anything, shoot straight forward
            // You can uncomment this if you want shooting without aiming at something
            // SpawnBullet(Camera.main.transform.position + Camera.main.transform.forward * maxDistance);
        }
    }

    void SpawnBullet(Vector3 targetPoint)
    {
        GameObject spawnedObject = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector3 direction = (targetPoint - transform.position).normalized;
        StartCoroutine(MoveObject(spawnedObject, direction));
    }

    private IEnumerator MoveObject(GameObject obj, Vector3 direction)
    {
        float traveledDistance = 0f;

        // Move the bullet until it reaches max distance
        while (traveledDistance < maxDistance && obj != null)
        {
            float step = speed * Time.deltaTime;
            obj.transform.Translate(direction * step, Space.World);
            traveledDistance += step;
            yield return null;
        }

        // Destroy the bullet after it has traveled the specified distance
        if (obj != null)
        {
            GameObject particle = SpawnParticle(particlePrefab, obj.transform.position);
            Destroy(particle, 2f);
            Destroy(obj);
        }
    }

    public GameObject SpawnParticle(ParticleSystem particlePrefab, Vector3 position)
    {
        // Instantiate the particle system at the given position
        ParticleSystem newParticle = Instantiate(particlePrefab, position, Quaternion.identity);

        // Play the particle system
        newParticle.Play();
        return newParticle.gameObject;
    }

    // Called by MainMenuManager to enable/disable shooting
    public void SetShootingEnabled(bool enabled)
    {
        canShoot = enabled;
    }
}