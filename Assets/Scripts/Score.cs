using UnityEngine;
using TMPro;
using System.Collections;

public class Score : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI multiplierText;

    [Header("Score Settings")]
    public int baseScorePerSecond = 1;

    private int score = 0;
    private int scoreMultiplier = 1;

    private Coroutine multiplierRoutine;

    void Start()
    {
        score = 0;
        UpdateUI();
        UpdateMultiplierUI();
    }

    void Update()
    {
        score += baseScorePerSecond * scoreMultiplier;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    void UpdateMultiplierUI()
    {
        if (multiplierText == null) return;

        if (scoreMultiplier > 1)
        {
            multiplierText.gameObject.SetActive(true);
            multiplierText.text = "Score Multiplier x" + scoreMultiplier;
        }
        else
        {
            multiplierText.gameObject.SetActive(false);
        }
    }

    // ---------------- MULTIPLIER LOGIC ----------------

    public void SetMultiplierForDuration(int multiplier, float duration)
    {
        if (multiplierRoutine != null)
        {
            StopCoroutine(multiplierRoutine);
        }

        multiplierRoutine = StartCoroutine(MultiplierTimer(multiplier, duration));
    }

    private IEnumerator MultiplierTimer(int multiplier, float duration)
    {
        scoreMultiplier = multiplier;
        UpdateMultiplierUI();

        yield return new WaitForSeconds(duration);

        scoreMultiplier = 1;
        UpdateMultiplierUI();
        multiplierRoutine = null;
    }
}
