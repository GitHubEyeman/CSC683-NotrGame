using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Prefab")]
    public GameObject collectPrefab;

    [Header("Lane Settings (Must Match Player)")]
    public int numberOfLanes = 5;
    public float laneWidth = 3f;

    [Header("Spawn Settings")]
    public float spawnInterval = 0.5f;
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
        int randomLane = Random.Range(0, numberOfLanes);
        float xPos = CalculateLanePosition(randomLane);

        Vector3 spawnPos = new Vector3(xPos, spawnY, spawnZ);
        Instantiate(collectPrefab, spawnPos, Quaternion.identity);
    }

    float CalculateLanePosition(int laneIndex)
    {
        float leftMost = -((numberOfLanes - 1) * laneWidth) / 2f;
        return leftMost + laneIndex * laneWidth;
    }
}
