using UnityEngine;
using UnityEngine.UI;

public class NewCrosshair : MonoBehaviour
{
    [Header("Crosshair Settings")]
    public Image crosshairImage;
    public float crosshairSize = 32f;
    
    [Header("Shooting Settings")]
    public LayerMask shootableLayers;
    
    private RectTransform crosshairRect;
    private Camera mainCamera;
    
    void Start()
    {
        // Get references
        if (crosshairImage != null)
        {
            crosshairRect = crosshairImage.GetComponent<RectTransform>();
            crosshairImage.gameObject.SetActive(false);
        }
        
        mainCamera = Camera.main;
        
        // Set crosshair size
        if (crosshairRect != null)
        {
            crosshairRect.sizeDelta = new Vector2(crosshairSize, crosshairSize);
        }
    }
    
    void Update()
    {
        // Update based on game state
        if (GameManager.Instance != null)
        {
            bool shouldShow = GameManager.Instance.isGameRunning && !GameManager.Instance.isGamePaused;
            
            if (crosshairImage != null)
            {
                crosshairImage.gameObject.SetActive(shouldShow);
                
                if (shouldShow)
                {
                    // Center crosshair on screen
                    crosshairRect.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                    
                    // Update cursor state
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }
    
    // Get the point where we're aiming (center of screen)
    public Vector3 GetAimPoint()
    {
        if (mainCamera == null) return Vector3.zero;
        
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, shootableLayers))
        {
            return hit.point;
        }
        
        // If nothing is hit, return a point far away in the camera's forward direction
        return ray.origin + ray.direction * 1000f;
    }
    
    // Check if we're aiming at something
    public bool IsAimingAtTarget()
    {
        if (mainCamera == null) return false;
        
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        return Physics.Raycast(ray, out hit, 1000f, shootableLayers);
    }
    
    // Get the transform of what we're aiming at
    public Transform GetAimedTarget()
    {
        if (mainCamera == null) return null;
        
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, shootableLayers))
        {
            return hit.transform;
        }
        
        return null;
    }
}