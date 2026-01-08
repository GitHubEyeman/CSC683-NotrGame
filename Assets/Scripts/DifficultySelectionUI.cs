using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultySelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button startButton;
    public Button backButton;
    public TextMeshProUGUI descriptionText;
    public GameObject difficultyPanel;

    void Start()
    {
        Debug.Log("DifficultySelectionUI Start()");
        
        // Setup button listeners
        easyButton.onClick.AddListener(() => SelectDifficulty("Easy"));
        normalButton.onClick.AddListener(() => SelectDifficulty("Normal"));
        hardButton.onClick.AddListener(() => SelectDifficulty("Hard"));
        startButton.onClick.AddListener(StartGame);
        backButton.onClick.AddListener(GoBackToMainMenu);
        
        // Set default selection
        SelectDifficulty("Normal");
    }

    void SelectDifficulty(string difficulty)
    {
        Debug.Log("Selected difficulty: " + difficulty);
        
        // Set description text
        switch (difficulty)
        {
            case "Easy":
                descriptionText.text = "EASY MODE\n\n• Slow obstacles\n• Few enemies\n• Low enemy health\n• Score x1";
                descriptionText.color = Color.green;
                break;
            case "Normal":
                descriptionText.text = "NORMAL MODE\n\n• Medium obstacles\n• Regular enemies\n• Normal enemy health\n• Score x2";
                descriptionText.color = Color.yellow;
                break;
            case "Hard":
                descriptionText.text = "HARD MODE\n\n• Fast obstacles\n• Many enemies\n• High enemy health\n• Score x3";
                descriptionText.color = Color.red;
                break;
        }
    }

    void StartGame()
    {
        Debug.Log("Start Game button clicked");
        
        // Find MainMenuManager
        MainMenuManager menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.OnGameStarted();
        }
        else
        {
            Debug.LogError("MainMenuManager not found!");
        }
    }

    void GoBackToMainMenu()
    {
        Debug.Log("Back button clicked");
        
        // Find MainMenuManager
        MainMenuManager menuManager = FindObjectOfType<MainMenuManager>();
        if (menuManager != null)
        {
            menuManager.ShowMainMenu();
        }
        
        // Hide difficulty panel
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);
    }
}