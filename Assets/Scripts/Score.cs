using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    
    [Header("Score Settings")]
    public int currentScore = 0;
    public int highScore = 0;
    public float scoreUpdateInterval = 0.1f;
    
    [Header("Difficulty Multiplier")]
    public bool useDifficultyMultiplier = true;
    
    private float scoreTimer = 0f;
    private bool isActive = false;

    void Start()
    {
        currentScore = 0;
        UpdateScoreDisplay();
        SetActive(false);
    }

    void Update()
    {
        if (!isActive || Time.timeScale == 0f)
            return;

        scoreTimer += Time.deltaTime;
        
        if (scoreTimer >= scoreUpdateInterval)
        {
            AddScore(1);
            scoreTimer = 0f;
        }
    }

    public void AddScore(int amount)
    {
        if (!isActive) return;

        if (useDifficultyMultiplier && DifficultyManager.Instance != null)
        {
            amount *= DifficultyManager.Instance.GetScoreMultiplier();
        }
        
        currentScore += amount;
        
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            string formattedScore = currentScore.ToString("N0");
            string multiplierText = "";
            
            if (useDifficultyMultiplier && DifficultyManager.Instance != null)
            {
                multiplierText = $" ×{DifficultyManager.Instance.GetScoreMultiplier()}";
            }
            
            scoreText.text = $"SCORE: {formattedScore}{multiplierText}";
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;
        
        if (active)
        {
            Debug.Log("Score system activated");
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreDisplay();
    }

    public int GetCurrentScore() { return currentScore; }
    public int GetHighScore() { return highScore; }
}