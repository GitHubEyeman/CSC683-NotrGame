using UnityEngine;

public class CollectMovement : MonoBehaviour
{
    public float speed = 25f;
    public float destroyZ = -10f;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
