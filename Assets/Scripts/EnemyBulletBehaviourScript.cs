using UnityEngine;

public class EnemyBulletBehaviourScript : MonoBehaviour
{
    public float lifeTime = 5f;
    public int damage = 10;


    void Start()
    {
        Destroy(gameObject, lifeTime);
    }


    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {

            PlayerHPLink player = other.gameObject.GetComponent<PlayerHPLink>();
            if (player != null)
            {
                Debug.Log("Player hit!");
                player.TakeDamage(damage);
            }
            Destroy(gameObject);

            
        }
    }
    
}
