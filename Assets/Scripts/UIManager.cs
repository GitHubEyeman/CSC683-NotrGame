using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Main Menu Buttons")]
    public Button playButton;
    public Button mainMenuQuitButton;
    
    [Header("Difficulty Menu Buttons")]
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button startButton;
    public Button backButton;
    
    [Header("Pause Menu Buttons")]
    public Button resumeButton;
    public Button pauseMenuButton;
    public Button pauseQuitButton;
    
    [Header("Game Over Buttons")]
    public Button restartButton;
    public Button gameOverMenuButton;
    public Button gameOverQuitButton;
    
    void Start()
    {
        AssignButtonListeners();
    }
    
    void AssignButtonListeners()
    {
        // Main Menu
        if (playButton != null)
            playButton.onClick.AddListener(() => GameManager.Instance.ShowDifficultySelection());
        
        if (mainMenuQuitButton != null)
            mainMenuQuitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        
        // Difficulty Menu
        if (easyButton != null)
            easyButton.onClick.AddListener(() => GameManager.Instance.SetEasyDifficulty());
        
        if (mediumButton != null)
            mediumButton.onClick.AddListener(() => GameManager.Instance.SetMediumDifficulty());
        
        if (hardButton != null)
            hardButton.onClick.AddListener(() => GameManager.Instance.SetHardDifficulty());
        
        if (startButton != null)
            startButton.onClick.AddListener(() => GameManager.Instance.StartGame());
        
        if (backButton != null)
            backButton.onClick.AddListener(() => GameManager.Instance.ShowMainMenu());
        
        // Pause Menu
        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => GameManager.Instance.TogglePause());
        
        if (pauseMenuButton != null)
            pauseMenuButton.onClick.AddListener(() => GameManager.Instance.ShowMainMenu());
        
        if (pauseQuitButton != null)
            pauseQuitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
        
        // Game Over Menu
        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        
        if (gameOverMenuButton != null)
            gameOverMenuButton.onClick.AddListener(() => GameManager.Instance.ShowMainMenu());
        
        if (gameOverQuitButton != null)
            gameOverQuitButton.onClick.AddListener(() => GameManager.Instance.QuitGame());
    }
}