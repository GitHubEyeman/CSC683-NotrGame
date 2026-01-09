using UnityEngine;

public class DoubleScorePickup : MonoBehaviour
{
    public int multiplier = 2;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Score score = FindFirstObjectByType<Score>();

            if (score != null)
            {
                score.SetMultiplierForDuration(multiplier, duration);
            }

            Destroy(gameObject);
        }
    }
}
