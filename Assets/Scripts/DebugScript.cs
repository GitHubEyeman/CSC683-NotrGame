using UnityEngine;

public class DebugScript : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== GAME STARTED ===");
        CheckGameState();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            CheckGameState();
        }
    }
    
    void CheckGameState()
    {
        Debug.Log("=== DEBUG INFO ===");
        
        // Check GameManager
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager Instance is NULL!");
        }
        else
        {
            Debug.Log($"GameManager: isGameRunning={GameManager.Instance.isGameRunning}, isGamePaused={GameManager.Instance.isGamePaused}");
        }
        
        // Check Player
        PlayerMovementScript player = FindFirstObjectByType<PlayerMovementScript>();
        if (player != null)
            Debug.Log($"Player Movement enabled: {player.enabled}");
        else
            Debug.LogError("PlayerMovementScript not found!");
        
        // Check Spawners
        ObstacleSpawner obstacle = FindFirstObjectByType<ObstacleSpawner>();
        Debug.Log($"ObstacleSpawner enabled: {obstacle != null && obstacle.enabled}");
        
        // Check Cursor
        Debug.Log($"Cursor: visible={Cursor.visible}, lockState={Cursor.lockState}");
        
        Debug.Log("=== END DEBUG ===");
    }
}