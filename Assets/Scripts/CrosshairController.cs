using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    [Header("Crosshair References")]
    public GameObject crosshair;  // Reference to your crosshair UI Image
    public Sprite defaultCrosshair; // Your Crosshair_26 sprite
    
    [Header("Cyberpunk Style Settings")]
    public Color crosshairColor = new Color(0, 1, 1, 1); // Cyan for cyberpunk
    public float crosshairScale = 1.0f;
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.2f;
    
    private RectTransform crosshairRectTransform;
    private Image crosshairImage;
    private bool isInGameplay = false;
    private Vector3 originalScale;
    private float pulseTimer = 0f;

    void Start()
    {
        InitializeCrosshair();
    }

    void InitializeCrosshair()
    {
        if (crosshair != null)
        {
            crosshairRectTransform = crosshair.GetComponent<RectTransform>();
            crosshairImage = crosshair.GetComponent<Image>();
            
            if (crosshairRectTransform == null)
            {
                Debug.LogError("Crosshair GameObject doesn't have RectTransform!");
                return;
            }
            
            // Set up the crosshair with your cyberpunk style
            SetupCyberpunkCrosshair();
            
            // Hide initially
            SetCrosshairActive(false);
        }
        else
        {
            Debug.LogWarning("Crosshair reference not set in CrosshairController!");
        }
    }

    void SetupCyberpunkCrosshair()
    {
        // Set your custom sprite
        if (defaultCrosshair != null && crosshairImage != null)
        {
            crosshairImage.sprite = defaultCrosshair;
        }
        
        // Set cyberpunk color (cyan)
        if (crosshairImage != null)
        {
            crosshairImage.color = crosshairColor;
        }
        
        // Set size
        if (crosshairRectTransform != null)
        {
            originalScale = crosshairRectTransform.localScale;
            crosshairRectTransform.localScale = originalScale * crosshairScale;
        }
        
        // Make sure it's visible
        if (crosshairImage != null)
        {
            crosshairImage.enabled = true;
            crosshairImage.raycastTarget = false; // Don't block clicks
        }
    }

    void Update()
    {
        if (isInGameplay && crosshair != null && crosshair.activeSelf)
        {
            UpdateCrosshairPosition();
            UpdateCyberpunkEffects();
        }
    }

    void UpdateCrosshairPosition()
    {
        // Get the current mouse position
        Vector3 mousePosition = Input.mousePosition;
        
        // Update crosshair position to follow mouse
        crosshairRectTransform.position = mousePosition;
    }

    void UpdateCyberpunkEffects()
    {
        // Add pulsing effect for cyberpunk feel
        pulseTimer += Time.deltaTime * pulseSpeed;
        
        float pulse = Mathf.Sin(pulseTimer) * pulseIntensity + 1f;
        
        if (crosshairRectTransform != null)
        {
            crosshairRectTransform.localScale = originalScale * crosshairScale * pulse;
        }
        
        // Optional: Color pulse effect
        if (crosshairImage != null)
        {
            Color pulseColor = crosshairColor;
            pulseColor.r *= pulse;
            pulseColor.g *= pulse;
            pulseColor.b *= pulse;
            crosshairImage.color = pulseColor;
        }
    }

    // Called by MainMenuManager when entering/leaving gameplay
    public void SetGameplayState(bool inGameplay)
    {
        isInGameplay = inGameplay;
        SetCrosshairActive(inGameplay);
        
        if (inGameplay)
        {
            // Hide system cursor, show custom crosshair
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            
            // Reset pulse timer
            pulseTimer = 0f;
        }
        else
        {
            // Show system cursor, hide custom crosshair
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void SetCrosshairActive(bool active)
    {
        if (crosshair != null)
        {
            crosshair.SetActive(active);
        }
    }

    // Public methods to customize crosshair during gameplay
    public void ChangeCrosshairColor(Color newColor)
    {
        crosshairColor = newColor;
        if (crosshairImage != null)
        {
            crosshairImage.color = crosshairColor;
        }
    }
    
    public void ChangeCrosshairScale(float newScale)
    {
        crosshairScale = newScale;
        if (crosshairRectTransform != null)
        {
            crosshairRectTransform.localScale = originalScale * crosshairScale;
        }
    }
}