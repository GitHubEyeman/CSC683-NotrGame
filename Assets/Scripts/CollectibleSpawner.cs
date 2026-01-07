using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collect Prefab")]
    public GameObject collectPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.5f;
    public float spawnRangeX = 6f;
    public float spawnY = 1f;
    public float spawnZ = 200f;

    void Start()
    {
        if (collectPrefab == null)
        {
            Debug.LogError("CollectibleSpawner: Collectible prefab not assigned.", this);
            enabled = false;
            return;
        }

        InvokeRepeating(nameof(SpawnCollect), 0f, spawnInterval);
    }

    void SpawnCollect()
    {
        float x = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(x, spawnY, spawnZ);

        Instantiate(collectPrefab, spawnPos, Quaternion.identity);
    }
}
