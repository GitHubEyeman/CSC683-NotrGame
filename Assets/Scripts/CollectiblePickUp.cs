using UnityEngine;

public class CollectiblePickUp : MonoBehaviour
{
    public int value = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Example: add to score manager
            // ScoreManager.Instance.AddCoins(value);
            // asalnya benda ni utk coins tapi aku tak tau nak buat caner sorry

            Destroy(gameObject);
        }
    }
}
