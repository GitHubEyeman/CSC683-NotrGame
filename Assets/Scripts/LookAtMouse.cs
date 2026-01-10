using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    public Camera mainCamera; // Assign your camera in the inspector
    public float maxDistance = 100f; // Max distance for the raycast

    void Update()
    {
        // Create a ray from the camera through the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Get the point where the ray hit
            Vector3 targetPoint = hit.point;

            // Keep the object level on the y-axis
            targetPoint.y = transform.position.y;

            // Rotate the object to look at the target point
            transform.LookAt(targetPoint);
        }
    }
}
