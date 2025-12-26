using System.Collections;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{


    public GameObject bullet;
    public float maxDistance = 50f;
    public float speed = 15f;

    public float spawnInterval = 0.3f;  
    private float timeSinceLastSpawn = 0f;  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (Input.GetMouseButton(0) && timeSinceLastSpawn >= spawnInterval)
        {
            timeSinceLastSpawn = 0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !hit.collider.CompareTag("Player"))
            {
                GameObject spawnedObject = Instantiate(bullet, transform.position, Quaternion.identity);
                Vector3 direction = (hit.point - transform.position).normalized;
                StartCoroutine(MoveObject(spawnedObject, direction));
            } else
            {
                //GameObject spawnedObject = Instantiate(bullet, transform.position, Quaternion.identity);
                //Vector3 direction = Camera.main.transform.forward.normalized;
                //StartCoroutine(MoveObject(spawnedObject, direction));
            }
            
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
            Destroy(obj);
        }
    }


}
