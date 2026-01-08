using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject difficultyPanel;
    public GameObject gameUI; // Your in-game HUD

    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button quitButton;

    [Header("Game Control")]
    public GameObject player;
    public CrosshairController crosshairController;
    public ShooterScript shooterScript;
    public Score scoreSystem; // Add this reference
    
    [Header("Spawners")]
    public ObstacleSpawner obstacleSpawner;
    public Spawner enemySpawner;
    public CollectibleSpawner collectibleSpawner;

    void Start()
    {
        Debug.Log("=== GAME STARTING ===");
        
        // Ensure time is running (for UI)
        Time.timeScale = 1f;
        
        // Show main menu, hide everything else
        ShowMainMenu();
        
        // Setup button listeners
        if (playButton != null)
            playButton.onClick.AddListener(ShowDifficultySelection);
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
        
        // Make sure cursor is visible for menu
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Disable all gameplay immediately
        DisableAllGameplay();
    }

    public void ShowMainMenu()
    {
        Debug.Log("Showing Main Menu");
        
        // Show main menu, hide everything else
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
        if (gameUI != null) gameUI.SetActive(false);

        // Show and unlock cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Tell crosshair controller we're in menu
        if (crosshairController != null)
        {
            crosshairController.SetGameplayState(false);
        }
        
        // Disable gameplay
        DisableAllGameplay();
    }

    public void ShowDifficultySelection()
    {
        Debug.Log("Showing Difficulty Selection");
        
        // Hide main menu, show difficulty selection
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(true);
        
        // Still show cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        Debug.Log("=== STARTING GAME ===");
        
        // Hide all menus
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (difficultyPanel != null) difficultyPanel.SetActive(false);
        
        // Show game UI
        if (gameUI != null) gameUI.SetActive(true);
        
        // Hide and lock cursor for gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        // Tell crosshair controller we're now in gameplay
        if (crosshairController != null)
        {
            crosshairController.SetGameplayState(true);
        }
        
        // Enable gameplay
        EnableAllGameplay();
    }

    void DisableAllGameplay()
    {
        Debug.Log("Disabling All Gameplay");
        
        // Disable player movement
        if (player != null)
        {
            PlayerMovementScript playerMovement = player.GetComponent<PlayerMovementScript>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
                Debug.Log("Player movement disabled");
            }
        }
        
        // Disable shooting
        if (shooterScript != null)
        {
            shooterScript.canShoot = false;
            Debug.Log("Shooting disabled");
        }
        
        // Disable crosshair
        if (crosshairController != null && crosshairController.crosshair != null)
        {
            crosshairController.crosshair.SetActive(false);
            Debug.Log("Crosshair disabled");
        }
        
        // Disable score system
        if (scoreSystem != null)
        {
            scoreSystem.SetActive(false);
            Debug.Log("Score system disabled");
        }
        
        // Disable all spawners
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = false;
            Debug.Log("ObstacleSpawner disabled");
        }
        
        if (enemySpawner != null)
        {
            enemySpawner.enabled = false;
            Debug.Log("EnemySpawner disabled");
        }
        
        if (collectibleSpawner != null)
        {
            collectibleSpawner.enabled = false;
            Debug.Log("CollectibleSpawner disabled");
        }
        
        // Pause the game
        Time.timeScale = 0f;
    }

    void EnableAllGameplay()
    {
        Debug.Log("Enabling All Gameplay");
        
        // Resume game time
        Time.timeScale = 1f;
        
        // Enable player movement
        if (player != null)
        {
            PlayerMovementScript playerMovement = player.GetComponent<PlayerMovementScript>();
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                Debug.Log("Player movement enabled");
            }
        }
        
        // Enable shooting
        if (shooterScript != null)
        {
            shooterScript.canShoot = true;
            Debug.Log("Shooting enabled");
        }
        
        // Enable crosshair
        if (crosshairController != null && crosshairController.crosshair != null)
        {
            crosshairController.crosshair.SetActive(true);
            Debug.Log("Crosshair enabled");
        }
        
        // Enable score system
        if (scoreSystem != null)
        {
            scoreSystem.SetActive(true);
            Debug.Log("Score system enabled");
        }
        
        // Enable all spawners
        if (obstacleSpawner != null)
        {
            obstacleSpawner.enabled = true;
            Debug.Log("ObstacleSpawner enabled");
        }
        
        if (enemySpawner != null)
        {
            enemySpawner.enabled = true;
            Debug.Log("EnemySpawner enabled");
        }
        
        if (collectibleSpawner != null)
        {
            collectibleSpawner.enabled = true;
            Debug.Log("CollectibleSpawner enabled");
        }
        
        // Enable DifficultyManager
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetGameActive(true);
            Debug.Log("DifficultyManager enabled");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // Call this from DifficultySelectionUI
    public void OnGameStarted()
    {
        StartGame();
    }
}