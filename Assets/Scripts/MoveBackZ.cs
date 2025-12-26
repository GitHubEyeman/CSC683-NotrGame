using UnityEngine;

public class MoveBackZ : MonoBehaviour
{

    public float speed = 25f;
    public float repositionDistance = 50f;
    public float spawnDistance = 160f;
    public bool destroyOnOutOfBounds = false;
    public GameObject respawnOnDestroy;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < -repositionDistance && !destroyOnOutOfBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnDistance);
        }
        else if (transform.position.z < -repositionDistance && destroyOnOutOfBounds) 
        { 
            
            if (respawnOnDestroy != null)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnDistance);
                Instantiate(respawnOnDestroy, pos, Quaternion.identity);
            }
            Destroy(gameObject);
        }

    }
}
