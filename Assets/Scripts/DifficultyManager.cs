using UnityEngine;
using System;

[Serializable]
public class DifficultySettings
{
    public string difficultyName;
    public float obstacleSpawnRate;
    public float enemySpawnRate;
    public int playerMaxHealth;
    public float enemyHealthMultiplier;
}

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;
    
    [Header("Difficulty Settings")]
    public DifficultySettings easySettings;
    public DifficultySettings mediumSettings;
    public DifficultySettings hardSettings;
    
    [Header("Current Difficulty")]
    public string currentDifficulty = "Medium";
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Set default values
        easySettings = new DifficultySettings
        {
            difficultyName = "Easy",
            obstacleSpawnRate = 2.0f,
            enemySpawnRate = 5.0f,
            playerMaxHealth = 150,
            enemyHealthMultiplier = 0.75f
        };
        
        mediumSettings = new DifficultySettings
        {
            difficultyName = "Medium",
            obstacleSpawnRate = 1.5f,
            enemySpawnRate = 3.0f,
            playerMaxHealth = 100,
            enemyHealthMultiplier = 1.0f
        };
        
        hardSettings = new DifficultySettings
        {
            difficultyName = "Hard",
            obstacleSpawnRate = 1.0f,
            enemySpawnRate = 1.5f,
            playerMaxHealth = 75,
            enemyHealthMultiplier = 1.5f
        };
    }
    
    public void ApplyEasyDifficulty()
    {
        ApplyDifficulty(easySettings);
    }
    
    public void ApplyMediumDifficulty()
    {
        ApplyDifficulty(mediumSettings);
    }
    
    public void ApplyHardDifficulty()
    {
        ApplyDifficulty(hardSettings);
    }
    
    void ApplyDifficulty(DifficultySettings settings)
    {
        currentDifficulty = settings.difficultyName;
        
        // Apply to spawners
        ObstacleSpawner obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        if (obstacleSpawner != null)
            obstacleSpawner.spawnInterval = settings.obstacleSpawnRate;
        
        EnemySpawner enemySpawner = FindFirstObjectByType<EnemySpawner>();
        if (enemySpawner != null)
            enemySpawner.spawnInterval = settings.enemySpawnRate;
        
        // Apply to player health
        HealthBar healthBar = FindFirstObjectByType<HealthBar>();
        if (healthBar != null)
            healthBar.maxHealth = settings.playerMaxHealth;
        
        Debug.Log($"Difficulty set to: {settings.difficultyName}");
    }
    
    public DifficultySettings GetCurrentSettings()
    {
        return currentDifficulty switch
        {
            "Easy" => easySettings,
            "Hard" => hardSettings,
            _ => mediumSettings, // Default to Medium
        };
    }
}