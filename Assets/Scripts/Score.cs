using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    private int highScore;
    private int MinScore = 0;
    
    [Header("Score Popup")]
    public GameObject scorePopupPrefab;
    public Transform popupSpawnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = MinScore;
        
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Only increase score when game is running and not paused
        if (GameManager.Instance != null && GameManager.Instance.isGameRunning && !GameManager.Instance.isGamePaused)
        {
            if (Time.timeScale == 1)
                score += 1; // Base score for survival
        }
        
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        
        // Update high score
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
    
    // Public method to add score from other scripts
    public void AddScore(int points, Vector3 position)
    {
        score += points;
        
        // Create score popup if available
        if (scorePopupPrefab != null)
        {
            GameObject popup = Instantiate(scorePopupPrefab, position, Quaternion.identity);
            TextMeshPro popupText = popup.GetComponentInChildren<TextMeshPro>();
            if (popupText != null)
            {
                popupText.text = $"+{points}";
            }
            Destroy(popup, 1f);
        }
    }
    
    // Get current score
    public int GetCurrentScore()
    {
        return score;
    }
    
    // Get high score
    public int GetHighScore()
    {
        return highScore;
    }
    
    // Reset score (for new game)
    public void ResetScore()
    {
        score = MinScore;
    }
}