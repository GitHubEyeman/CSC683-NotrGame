using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
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
        score += 1;
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
