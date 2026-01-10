using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoToMainScene()
    {
       SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        print("Quit");
        Application.Quit();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

     public void DifficultySelect()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("DifficultyMenu");
    }

    public void GoToEasy()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Easy");
    }

    public void GoToMedium()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Medium");
    }
    public void GoToHard()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Hard");
    }
}
