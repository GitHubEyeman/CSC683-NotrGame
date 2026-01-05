using UnityEngine;

public class FloatNrotate : MonoBehaviour
{
    public float floatStrength = 0.5f;  // How high the item floats
    public float floatSpeed = 1.0f;    // Speed of the floating motion
    public float rotateSpeed = 30.0f;  // Speed of rotation

    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position of the item
        originalPosition = transform.position;
    }

    void Update()
    {
        // Make the item float up and down (sine wave movement)
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatStrength;
        transform.position = new Vector3(originalPosition.x, originalPosition.y + newY, transform.position.z);

        // Make the item rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
