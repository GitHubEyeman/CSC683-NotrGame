using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshair;  // Reference to your crosshair sprite
    private bool isCrosshairActive = true;  // To track if the crosshair is active
    private RectTransform crosshairRectTransform;

    void Start()
    {
        // Initialize the crosshair and hide the system cursor
        Cursor.visible = false;
        crosshair.SetActive(true);

        // Get the RectTransform of the crosshair
        crosshairRectTransform = crosshair.GetComponent<RectTransform>();
    }

    void Update()
    {
        // If the crosshair is active, update its position to follow the mouse
        if (isCrosshairActive)
        {
            Vector3 mousePos = Input.mousePosition;  // Get the mouse position in screen space
            crosshairRectTransform.position = mousePos;  // Set the crosshair's position to the mouse position
        }

        // Toggle visibility when ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ToggleCursorAndCrosshair();
        }
    }

    public void ToggleCursorAndCrosshair()
    {
        isCrosshairActive = !isCrosshairActive;

        if (isCrosshairActive)
        {
            Cursor.visible = false;  // Hide the system cursor
            crosshair.SetActive(true);  // Show the crosshair
        }
        else
        {
            Cursor.visible = true;  // Show the system cursor
            crosshair.SetActive(false);  // Hide the crosshair
        }
    }
}
