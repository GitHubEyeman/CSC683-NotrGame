using UnityEngine;

public class CollectMovementScript : MonoBehaviour
{
    public float speed = 25f;
    public float destroyZ = -10f;

    void Update()
    {
        // Only move if game is running
        if (GameManager.Instance == null || !GameManager.Instance.isGameRunning || GameManager.Instance.isGamePaused)
            return;
            
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}