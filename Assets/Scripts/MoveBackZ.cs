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
        // Move the object in the global Z direction
        Vector3 newPosition = transform.position + Vector3.back * speed * Time.deltaTime;
        transform.position = newPosition;

        // Check if the object goes out of bounds and reposition or destroy it
        if (transform.position.z < -repositionDistance && !destroyOnOutOfBounds)
        {
            // Reposition the object to the spawn distance in the global Z position
            transform.position = new Vector3(transform.position.x, transform.position.y, spawnDistance);
        }
        else if (transform.position.z < -repositionDistance && destroyOnOutOfBounds)
        {
            // Destroy the object or respawn it at a new position
            if (respawnOnDestroy != null)
            {
                Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z + spawnDistance);
                Instantiate(respawnOnDestroy, pos, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
