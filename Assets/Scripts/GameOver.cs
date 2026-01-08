using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;


public class GameOver : MonoBehaviour
{
    private GameObject GameOverUI;

    public void OpenEndScreen()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }


   
}
