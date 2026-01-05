using UnityEngine;

public class PlayerPosTracker : MonoBehaviour
{

    public Transform player;

    

    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.position = player.position;
    }
}
