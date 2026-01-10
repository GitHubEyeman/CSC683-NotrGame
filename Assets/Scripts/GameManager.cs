using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public CrosshairController crosshairController;
    public HealthBar healthBar;
    public Score score;
    public TextMeshProUGUI endScoreText;

    private bool isPaused = false;
    private bool isGameOver = false;

    void Start()
    {
        // Make sure panels are hidden at start
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        // Toggle pause with ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        //Gameover when HP <=0
        if (healthBar.GetHealth() <= 0 && !isGameOver) GameOver();
    }

    // --------------------
    // PAUSE FUNCTIONS
    // --------------------

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        crosshairController.ToggleCursorAndCrosshair();
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        crosshairController.ToggleCursorAndCrosshair();
    }

    // --------------------
    // GAME OVER FUNCTIONS
    // --------------------

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        crosshairController.ToggleCursorAndCrosshair();
        endScoreText.text = "Score: " + score.score;


    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --------------------
    // COMMON BUTTONS
    // --------------------

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // <-- change if needed
    }

    public void QuitGame()
    {
        Application.Quit();

        // For testing in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}





