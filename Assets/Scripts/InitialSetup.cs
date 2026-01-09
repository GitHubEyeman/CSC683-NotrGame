using UnityEngine;

public class InitialSetup : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== INITIAL SETUP STARTING ===");
        
        // Make sure GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        
        // Make sure all game elements are disabled at start
        DisableAllGameplayElements();
        
        // Make sure only Main Menu is shown
        GameManager.Instance.ShowMainMenu();
        
        Debug.Log("=== INITIAL SETUP COMPLETE ===");
    }
    
    void DisableAllGameplayElements()
    {
        // Find and disable player
        PlayerMovementScript player = FindFirstObjectByType<PlayerMovementScript>();
        if (player != null)
        {
            player.enabled = false;
            Debug.Log("Player movement disabled");
        }
        
        // Find and disable shooter
        ShooterScript shooter = FindFirstObjectByType<ShooterScript>();
        if (shooter != null)
        {
            shooter.enabled = false;
            Debug.Log("Shooter disabled");
        }
        
        // Find and disable all spawners
        ObstacleSpawner obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false;
            obstacleSpawner.StopSpawning();
            Debug.Log("Obstacle spawner disabled");
        }
        
        EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.enabled = false;
            enemySpawner.StopSpawning();
            Debug.Log("Enemy spawner disabled");
        }
        
        CollectibleSpawner collectibleSpawner = FindFirstObjectByType<CollectibleSpawner>();
        if (collectibleSpawner != null)
        {
            collectibleSpawner.enabled = false;
            collectibleSpawner.StopSpawning();
            Debug.Log("Collectible spawner disabled");
        }
        
        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Debug.Log("Cursor shown and unlocked");
    }
}