using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Prefabs")]
    public GameObject[] collectiblePrefabs;

    [Header("Lane Settings (Must Match Player)")]
    public int numberOfLanes = 5;
    public float laneWidth = 3f;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.5f;
    public float spawnY = 1f;
    public float spawnZ = 200f;

    void Start()
    {
        if (collectiblePrefabs == null || collectiblePrefabs.Length == 0)
        {
            Debug.LogError("CollectibleSpawner: No collectible prefabs assigned.", this);
            enabled = false;
            return;
        }

        InvokeRepeating(nameof(SpawnCollect), 0f, spawnInterval);
    }

    void SpawnCollect()
    {
        int randomLane = Random.Range(0, numberOfLanes);
        float xPos = CalculateLanePosition(randomLane);

        GameObject prefab =
            collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];

        Vector3 spawnPos = new Vector3(xPos, spawnY, spawnZ);
        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    float CalculateLanePosition(int laneIndex)
    {
        float leftMost = -((numberOfLanes - 1) * laneWidth) / 2f;
        return leftMost + laneIndex * laneWidth;
    }
}
