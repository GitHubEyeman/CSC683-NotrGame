using UnityEngine;

public class EnemySMovementScript : MonoBehaviour
{
    public Transform target;  // The target object to face
    public float speed = 2f;  // Speed of the movement
    public float radiusX = 3f; // Size of the 8-shape path
    public float radiusY = 1f; // Size of the 8-shape path
    public float frequencyX = 1f; // Speed of the 8-shape oscillation
    public float frequencyY = 2f; // Speed of the 8-shape oscillation

    private Vector3 initialPosition;
    private float time;

    void Start()
    {
        // Store the object's initial position when the game starts
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the 8-shape path using sine and cosine functions
        time += Time.deltaTime * speed; // Increment time
        float x = radiusX * Mathf.Sin(time * frequencyX); // X movement (sin for 8-shape)
        float y = radiusY * Mathf.Sin(time * frequencyY); // Y movement (cos for 8-shape)

        // Move the object to the new position
        transform.position = new Vector3(initialPosition.x + x, initialPosition.y + y, transform.position.z);

        // Make the object face the target
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
