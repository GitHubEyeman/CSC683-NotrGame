using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public bool Gameover = false;

    public void RestartGame() { 
        // Reload the current scene to restart the game
        if Input.GetKeyDown(KeyCode.r & Gameover){
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().MainScene);
        }
    }

    private void Update()
    {
        RestartGame();
    }


}
