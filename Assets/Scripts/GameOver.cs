using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class GameOver : MonoBehaviour
{
    
    [SerializeField] private GameObject RestartBtn;
    [SerializeField] private GameObject MainMenuBtn;
    

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
}
