using UnityEngine;

public class GameSetup : MonoBehaviour
{
    void Start()
    {
        // Make sure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager instance is null! Make sure GameManager GameObject is in the scene.");
        }
        else
        {
            Debug.Log("GameManager found successfully!");
        }
        
        // Hide cursor and show crosshair based on game state
        UpdateCursorState();
    }
    
    void UpdateCursorState()
    {
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isGameRunning && !GameManager.Instance.isGamePaused)
            {
                // In game, hide cursor
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                // In menu, show cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    
    void Update()
    {
        UpdateCursorState();
    }
}