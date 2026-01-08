using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Header("Collectible Prefab")]
    public GameObject collectPrefab;

    [Header("Lane Settings (Must Match Player)")]
    public int numberOfLanes = 5;
    public float laneWidth = 3f;

    [Header("Spawn Settings")]
    public float spawnY = 1f;
    public float spawnZ = 200f;

    private float currentSpawnInterval = 0.5f;
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (collectPrefab == null)
        {
            Debug.LogError("CollectibleSpawner: Collectible prefab not assigned.", this);
            enabled = false;
            return;
        }

        // Get initial spawn rate from DifficultyManager
        if (DifficultyManager.Instance != null)
        {
            currentSpawnInterval = DifficultyManager.Instance.GetCollectibleSpawnRate();
        }

        StartSpawning();
    }

    void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnCollectibles());
    }

    System.Collections.IEnumerator SpawnCollectibles()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            SpawnCollect();
        }
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

    public void UpdateSpawnRate(float newSpawnRate)
    {
        currentSpawnInterval = newSpawnRate;
    }
}