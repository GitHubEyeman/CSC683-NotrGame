using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{


    public GameObject[] bullet;
    public ParticleSystem particlePrefab;
    public float maxDistance = 50f;
    public float speed = 15f;

    public float spawnInterval = 0.3f;  
    private float timeSinceLastSpawn = 0f;
    private float bulletUpgradeTime = 0f;
    private bool bulletUpgraded = false;
    private int currentBulletType = 0;
    public AudioSource shootingSound;
    public AudioSource blasterSound;
    public AudioSource powerUpSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        

        if (Input.GetMouseButton(0) && timeSinceLastSpawn >= spawnInterval && Time.timeScale == 1)
        {
            timeSinceLastSpawn = 0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player"))
            {
                GameObject spawnedObject = Instantiate(bullet[currentBulletType], transform.position, Quaternion.identity);
                Vector3 direction = (hit.point - transform.position).normalized;
                StartCoroutine(MoveObject(spawnedObject, direction));
                
                if (bulletUpgraded && blasterSound != null)
                {
                    blasterSound.Play();
                }
                else if (shootingSound != null)
                {
                    shootingSound.Play();
                }
                
            } else
            {
                //GameObject spawnedObject = Instantiate(bullet, transform.position, Quaternion.identity);
                //Vector3 direction = Camera.main.transform.forward.normalized;
                //StartCoroutine(MoveObject(spawnedObject, direction));
            }
            
        }

        if (bulletUpgraded) bulletUpgradeTime += Time.deltaTime;
        if (bulletUpgradeTime > 10f)
        {
            bulletUpgradeTime = 0f;
            bulletUpgraded = false;
            currentBulletType = 0;
        }
    }

    private IEnumerator MoveObject(GameObject obj, Vector3 direction)
    {
        float traveledDistance = 0f;

        // Track the object movement

        while (traveledDistance < maxDistance && obj != null)
        {
            float step = speed * Time.deltaTime; // Calculate the movement step per frame
            obj.transform.Translate(direction * step, Space.World);
            traveledDistance += step;

            yield return null;
        }

        // Destroy the object after it has traveled the specified distance
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

    public void upgradeBlaster(int blaster)
    {
        if (bulletUpgraded) bulletUpgradeTime = 0f;

        bulletUpgraded = true;
        currentBulletType = blaster;

        if (powerUpSound != null)
        {
            powerUpSound.Play();
        }

    }

}
