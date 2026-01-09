using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public GameObject crosshair;
    private RectTransform crosshairRectTransform;
    
    void Start()
    {
        if (crosshair != null)
        {
            crosshairRectTransform = crosshair.GetComponent<RectTransform>();
            crosshair.SetActive(false); // Start hidden
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void Update()
    {
        // Only show crosshair when game is running AND not paused
        if (GameManager.Instance != null)
        {
            bool shouldShowCrosshair = GameManager.Instance.isGameRunning && !GameManager.Instance.isGamePaused;
            
            if (crosshair != null)
            {
                if (shouldShowCrosshair && !crosshair.activeSelf)
                {
                    crosshair.SetActive(true);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else if (!shouldShowCrosshair && crosshair.activeSelf)
                {
                    crosshair.SetActive(false);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
            
            // Update crosshair position
            if (shouldShowCrosshair && crosshair != null && crosshair.activeSelf)
            {
                Vector3 mousePos = Input.mousePosition;
                crosshairRectTransform.position = mousePos;
            }
        }
    }
}