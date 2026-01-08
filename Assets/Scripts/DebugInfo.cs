using UnityEngine;

public class DebugInfo : MonoBehaviour
{
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 400, 30), "Time Scale: " + Time.timeScale);
        GUI.Label(new Rect(10, 40, 400, 30), "Cursor Visible: " + Cursor.visible);
        GUI.Label(new Rect(10, 70, 400, 30), "Cursor Lock: " + Cursor.lockState);
        GUI.Label(new Rect(10, 100, 400, 30), "Player Movement: " + FindObjectOfType<PlayerMovementScript>().enabled);
        GUI.Label(new Rect(10, 130, 400, 30), "Shooting Enabled: " + FindObjectOfType<ShooterScript>().canShoot);
    }
}