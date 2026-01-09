using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // Game States
    [Header("Game States")]
    public bool isGameRunning = false;
    public bool isGamePaused = false;
    
    // Difficulty enum
    public enum Difficulty { Easy, Medium, Hard }
    
    [Header("Difficulty Settings")]
    public Difficulty currentDifficulty = Difficulty.Medium;
    
    [Header("Difficulty Parameters")]
    public float easyObstacleSpawnRate = 2.0f;
    public float mediumObstacleSpawnRate = 1.5f;
    public float hardObstacleSpawnRate = 1.0f;
    
    public float easyEnemySpawnRate = 8f;
    public float mediumEnemySpawnRate = 5f;
    public float hardEnemySpawnRate = 3f;
    
    public float easyCollectibleSpawnRate = 5f;
    public float mediumCollectibleSpawnRate = 3f;
    public float hardCollectibleSpawnRate = 2f;
    
    [Header("UI References")]
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject hudPanel;
    
    [Header("UI Text")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI difficultyText;
    
    private Score scoreScript;
    private PlayerMovementScript playerMovement;
    private ShooterScript shooterScript;
    private ObstacleSpawner obstacleSpawner;
    private EnemySpawner enemySpawner;
    private CollectibleSpawner collectibleSpawner;
    
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
        Debug.Log("GameManager Started");
        
        // Find all scripts
        FindAllScripts();
        EnableGameplay();
        // Initial state - NOTHING should be running
        // ForceStopEverything();
        
        // Show main menu
        // ShowMainMenu();
        
        // Make sure cursor is visible
    }
    
    void FindAllScripts()
    {
        scoreScript = FindFirstObjectByType<Score>();
        playerMovement = FindFirstObjectByType<PlayerMovementScript>();
        shooterScript = FindFirstObjectByType<ShooterScript>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        collectibleSpawner = FindFirstObjectByType<CollectibleSpawner>();
        
        Debug.Log("Scripts found: " +
                  $"Player: {playerMovement != null}, " +
                  $"Shooter: {shooterScript != null}, " +
                  $"ObstacleSpawner: {obstacleSpawner != null}, " +
                  $"EnemySpawner: {enemySpawner != null}, " +
                  $"CollectibleSpawner: {collectibleSpawner != null}");
    }
    
    void ForceStopEverything()
    {
        Debug.Log("Force stopping everything...");
        
        // Stop time
        Time.timeScale = 0f;
        
        // Disable player
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Debug.Log("Player movement forced OFF");
        }
        
        // Disable shooter
        if (shooterScript != null)
        {
            shooterScript.enabled = false;
            Debug.Log("Shooter forced OFF");
        }
        
        // Stop all spawners
        if (obstacleSpawner != null)
        {
            obstacleSpawner.StopSpawning();
            obstacleSpawner.enabled = false;
            Debug.Log("Obstacle spawner forced OFF");
        }
        
        if (enemySpawner != null)
        {
            enemySpawner.StopSpawning();
            enemySpawner.enabled = false;
            Debug.Log("Enemy spawner forced OFF");
        }
        
        if (collectibleSpawner != null)
        {
            collectibleSpawner.StopSpawning();
            collectibleSpawner.enabled = false;
            Debug.Log("Collectible spawner forced OFF");
        }
    }
    
    void Update()
    {
        // Only handle ESC if game is running
        if (isGameRunning && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Debug key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DebugState();
        }
    }
    
    // ===== MENU FUNCTIONS =====
    public void ShowMainMenu()
    {
        Debug.Log("Showing Main Menu");
        
        // Hide all other panels
        SetPanelActive(mainMenuPanel, true);
        SetPanelActive(difficultyPanel, false);
        SetPanelActive(pausePanel, false);
        SetPanelActive(gameOverPanel, false);
        SetPanelActive(hudPanel, false);
        
        // Game is NOT running
        isGameRunning = false;
        isGamePaused = false;
        
        // Force stop everything
        ForceStopEverything();
        
        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        Debug.Log("Game is in Main Menu state - NOTHING should be running");
    }
    
    public void ShowDifficultySelection()
    {
        Debug.Log("Showing Difficulty Selection");
        
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(difficultyPanel, true);
    }
    
    public void StartGame()
    {
        Debug.Log($"=== STARTING GAME with difficulty: {currentDifficulty} ===");
        
        // Stop everything first (just to be sure)
        //ForceStopEverything();
        
        // Now set time to normal
        Time.timeScale = 1f;
        
        // Set game states
        isGameRunning = true;
        isGamePaused = false;
        
        // Show HUD, hide other panels
        SetPanelActive(mainMenuPanel, false);
        SetPanelActive(difficultyPanel, false);
        SetPanelActive(pausePanel, false);
        SetPanelActive(gameOverPanel, false);
        SetPanelActive(hudPanel, true);
        
        // Apply difficulty settings BEFORE enabling anything
        ApplyDifficultySettings();
        
        // ENABLE EVERYTHING FOR GAMEPLAY
        EnableGameplay();
        
        // Hide cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        Debug.Log("Game started successfully!");
    }
    
    void EnableGameplay()
    {
        Debug.Log("Enabling gameplay elements...");
        
        // Enable player
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
            Debug.Log("Player movement enabled");
        }
        
        // Enable shooter
        if (shooterScript != null)
        {
            shooterScript.enabled = true;
            Debug.Log("Shooter enabled");
        }
        
        // Enable and start spawners
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = true;
            obstacleSpawner.StartSpawning();
            Debug.Log($"Obstacle spawner enabled, interval: {obstacleSpawner.spawnInterval}");
        }
        
        if (enemySpawner != null)
        {
            enemySpawner.enabled = true;
            enemySpawner.StartSpawning();
            Debug.Log($"Enemy spawner enabled, interval: {enemySpawner.spawnInterval}");
        }
        
        if (collectibleSpawner != null)
        {
            collectibleSpawner.enabled = true;
            collectibleSpawner.StartSpawning();
            Debug.Log($"Collectible spawner enabled, interval: {collectibleSpawner.spawnInterval}");
        }
        
        // Reset score
        if (scoreScript != null)
        {
            scoreScript.score = 0;
            Debug.Log("Score reset to 0");
        }
    }
    
    public void TogglePause()
    {
        if (!isGameRunning) return;
        
        isGamePaused = !isGamePaused;
        SetPanelActive(pausePanel, isGamePaused);
        SetPanelActive(hudPanel, !isGamePaused);
        
        if (isGamePaused)
        {
            // Pause everything
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Game Paused");
        }
        else
        {
            // Resume everything
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Game Resumed");
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over!");
        
        isGameRunning = false;
        SetPanelActive(gameOverPanel, true);
        SetPanelActive(hudPanel, false);
        
        // Show final score
        if (scoreScript != null && finalScoreText != null)
        {
            finalScoreText.text = "Score: " + scoreScript.score;
        }
        
        // Stop everything
        ForceStopEverything();
        
        // Show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    // ===== DIFFICULTY FUNCTIONS =====
    public void SetEasyDifficulty()
    {
        currentDifficulty = Difficulty.Easy;
        if (difficultyText != null)
            difficultyText.text = "DIFFICULTY: EASY";
        Debug.Log("Difficulty set to EASY");
    }
    
    public void SetMediumDifficulty()
    {
        currentDifficulty = Difficulty.Medium;
        if (difficultyText != null)
            difficultyText.text = "DIFFICULTY: MEDIUM";
        Debug.Log("Difficulty set to MEDIUM");
    }
    
    public void SetHardDifficulty()
    {
        currentDifficulty = Difficulty.Hard;
        if (difficultyText != null)
            difficultyText.text = "DIFFICULTY: HARD";
        Debug.Log("Difficulty set to HARD");
    }
    
    void ApplyDifficultySettings()
    {
        Debug.Log($"Applying {currentDifficulty} difficulty settings");
        
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                if (obstacleSpawner != null) obstacleSpawner.spawnInterval = easyObstacleSpawnRate;
                if (enemySpawner != null) enemySpawner.spawnInterval = easyEnemySpawnRate;
                if (collectibleSpawner != null) collectibleSpawner.spawnInterval = easyCollectibleSpawnRate;
                break;
                
            case Difficulty.Medium:
                if (obstacleSpawner != null) obstacleSpawner.spawnInterval = mediumObstacleSpawnRate;
                if (enemySpawner != null) enemySpawner.spawnInterval = mediumEnemySpawnRate;
                if (collectibleSpawner != null) collectibleSpawner.spawnInterval = mediumCollectibleSpawnRate;
                break;
                
            case Difficulty.Hard:
                if (obstacleSpawner != null) obstacleSpawner.spawnInterval = hardObstacleSpawnRate;
                if (enemySpawner != null) enemySpawner.spawnInterval = hardEnemySpawnRate;
                if (collectibleSpawner != null) collectibleSpawner.spawnInterval = hardCollectibleSpawnRate;
                break;
        }
    }
    
    // ===== UTILITY FUNCTIONS =====
    void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
    
    void DebugState()
    {
        Debug.Log("=== DEBUG STATE ===");
        Debug.Log($"Game Running: {isGameRunning}");
        Debug.Log($"Game Paused: {isGamePaused}");
        Debug.Log($"Time Scale: {Time.timeScale}");
        Debug.Log($"Player enabled: {playerMovement != null && playerMovement.enabled}");
        Debug.Log($"Shooter enabled: {shooterScript != null && shooterScript.enabled}");
        Debug.Log($"ObstacleSpawner enabled: {obstacleSpawner != null && obstacleSpawner.enabled}");
        Debug.Log($"Cursor visible: {Cursor.visible}, lockState: {Cursor.lockState}");
        Debug.Log("===================");
    }
    
    // ===== BUTTON FUNCTIONS =====
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        
        // Reset time scale
        Time.timeScale = 1f;
        
        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Re-initialize after scene reload
        StartCoroutine(InitializeAfterSceneLoad());
    }
    
    IEnumerator InitializeAfterSceneLoad()
    {
        // Wait one frame for scene to load
        yield return null;
        
        // Find scripts again
        FindAllScripts();
        
        // Start game with current difficulty
        StartGame();
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
            // If running in Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            // If running in built game
            Application.Quit();
        #endif
    }
}