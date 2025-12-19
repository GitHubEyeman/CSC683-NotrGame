using UnityEngine;

public class MoveBackZ : MonoBehaviour
{

    public float speed = 5f;
    public float destroyDistance = 50f;

    
    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
        
        if (transform.position.z < -destroyDistance)
        {
            Destroy(gameObject);
        }

    }
}
