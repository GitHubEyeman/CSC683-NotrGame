using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score = 0;
    private int highScore;
    private int MinScore = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = MinScore;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.timeScale == 1)
            score += 1;
        if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
    }
}
