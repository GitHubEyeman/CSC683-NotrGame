using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DifficultyManager : MonoBehaviour
{
    public enum DifficultyLevel { Easy, Normal, Hard }
    
    [Header("Current Difficulty")]
    public DifficultyLevel currentDifficulty = DifficultyLevel.Normal;
    
    [Header("Easy Settings")]
    public float easyObstacleSpawnRate = 2.0f;
    public float easyEnemySpawnRate = 5.0f;
    public float easyCollectibleSpawnRate = 0.5f;
    public int easyEnemyHealth = 3;
    public float easyObstacleSpeed = 10f;
    public int easyScoreMultiplier = 1;
    
    [Header("Normal Settings")]
    public float normalObstacleSpawnRate = 1.5f;
    public float normalEnemySpawnRate = 3.5f;
    public float normalCollectibleSpawnRate = 0.5f;
    public int normalEnemyHealth = 4;
    public float normalObstacleSpeed = 12f;
    public int normalScoreMultiplier = 2;
    
    [Header("Hard Settings")]
    public float hardObstacleSpawnRate = 1.0f;
    public float hardEnemySpawnRate = 2.0f;
    public float hardCollectibleSpawnRate = 0.5f;
    public int hardEnemyHealth = 6;
    public float hardObstacleSpeed = 15f;
    public int hardScoreMultiplier = 3;
    
    [Header("Difficulty Scaling Over Time")]
    public float difficultyIncreaseInterval = 30f;
    public float spawnRateDecreasePerInterval = 0.1f;
    public float speedIncreasePerInterval = 0.5f;
    
    // Singleton pattern for easy access
    public static DifficultyManager Instance { get; private set; }
    
    private float gameTime = 0f;
    private float timeSinceLastIncrease = 0f;
    private bool isGameActive = true;
    
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
    }
    
    void Start()
    {
        // Set initial difficulty
        ApplyDifficultySettings();
    }
    
    void Update()
    {
        if (!isGameActive) return;
        
        // Increase difficulty over time
        gameTime += Time.deltaTime;
        timeSinceLastIncrease += Time.deltaTime;
        
        if (timeSinceLastIncrease >= difficultyIncreaseInterval)
        {
            IncreaseDifficultyOverTime();
            timeSinceLastIncrease = 0f;
        }
    }
    
    void IncreaseDifficultyOverTime()
    {
        // Make obstacles and enemies spawn faster over time
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                easyObstacleSpawnRate = Mathf.Max(0.8f, easyObstacleSpawnRate - spawnRateDecreasePerInterval);
                easyEnemySpawnRate = Mathf.Max(2.0f, easyEnemySpawnRate - spawnRateDecreasePerInterval);
                easyObstacleSpeed += speedIncreasePerInterval;
                break;
                
            case DifficultyLevel.Normal:
                normalObstacleSpawnRate = Mathf.Max(0.5f, normalObstacleSpawnRate - spawnRateDecreasePerInterval);
                normalEnemySpawnRate = Mathf.Max(1.5f, normalEnemySpawnRate - spawnRateDecreasePerInterval);
                normalObstacleSpeed += speedIncreasePerInterval;
                break;
                
            case DifficultyLevel.Hard:
                hardObstacleSpawnRate = Mathf.Max(0.3f, hardObstacleSpawnRate - spawnRateDecreasePerInterval);
                hardEnemySpawnRate = Mathf.Max(1.0f, hardEnemySpawnRate - spawnRateDecreasePerInterval);
                hardObstacleSpeed += speedIncreasePerInterval;
                break;
        }
        
        ApplyDifficultySettings();
        Debug.Log($"Difficulty increased! Game Time: {Mathf.Round(gameTime)}s");
    }
    
    public void SetDifficulty(DifficultyLevel level)
    {
        currentDifficulty = level;
        ResetDynamicDifficulty();
        ApplyDifficultySettings();
        Debug.Log($"Difficulty set to: {level}");
    }
    
    public void SetGameActive(bool active)
    {
        isGameActive = active;
    }
    
    void ResetDynamicDifficulty()
    {
        // Reset to initial values
        gameTime = 0f;
        timeSinceLastIncrease = 0f;
        
        // Reset easy settings
        easyObstacleSpawnRate = 2.0f;
        easyEnemySpawnRate = 5.0f;
        easyObstacleSpeed = 10f;
        
        // Reset normal settings
        normalObstacleSpawnRate = 1.5f;
        normalEnemySpawnRate = 3.5f;
        normalObstacleSpeed = 12f;
        
        // Reset hard settings
        hardObstacleSpawnRate = 1.0f;
        hardEnemySpawnRate = 2.0f;
        hardObstacleSpeed = 15f;
    }
    
    void ApplyDifficultySettings()
    {
        // Notify all spawners and systems to update
        UpdateAllSpawners();
    }
    
    void UpdateAllSpawners()
    {
        // Find and update all spawners in the scene
        ObstacleSpawner obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        if (obstacleSpawner != null)
        {
            obstacleSpawner.UpdateSpawnRate(GetObstacleSpawnRate());
        }
        
        Spawner enemySpawner = FindObjectOfType<Spawner>();
        if (enemySpawner != null)
        {
            enemySpawner.UpdateSpawnRate(GetEnemySpawnRate());
        }
        
        CollectibleSpawner collectibleSpawner = FindObjectOfType<CollectibleSpawner>();
        if (collectibleSpawner != null)
        {
            collectibleSpawner.UpdateSpawnRate(GetCollectibleSpawnRate());
        }
    }
    
    // Public getters for other scripts
    public float GetObstacleSpawnRate()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyObstacleSpawnRate;
            case DifficultyLevel.Normal: return normalObstacleSpawnRate;
            case DifficultyLevel.Hard: return hardObstacleSpawnRate;
            default: return normalObstacleSpawnRate;
        }
    }
    
    public float GetEnemySpawnRate()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyEnemySpawnRate;
            case DifficultyLevel.Normal: return normalEnemySpawnRate;
            case DifficultyLevel.Hard: return hardEnemySpawnRate;
            default: return normalEnemySpawnRate;
        }
    }
    
    public float GetCollectibleSpawnRate()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyCollectibleSpawnRate;
            case DifficultyLevel.Normal: return normalCollectibleSpawnRate;
            case DifficultyLevel.Hard: return hardCollectibleSpawnRate;
            default: return normalCollectibleSpawnRate;
        }
    }
    
    public int GetEnemyHealth()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyEnemyHealth;
            case DifficultyLevel.Normal: return normalEnemyHealth;
            case DifficultyLevel.Hard: return hardEnemyHealth;
            default: return normalEnemyHealth;
        }
    }
    
    public float GetObstacleSpeed()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyObstacleSpeed;
            case DifficultyLevel.Normal: return normalObstacleSpeed;
            case DifficultyLevel.Hard: return hardObstacleSpeed;
            default: return normalObstacleSpeed;
        }
    }
    
    public int GetScoreMultiplier()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy: return easyScoreMultiplier;
            case DifficultyLevel.Normal: return normalScoreMultiplier;
            case DifficultyLevel.Hard: return hardScoreMultiplier;
            default: return normalScoreMultiplier;
        }
    }
}