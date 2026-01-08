using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Lane Settings (Must Match Player)")]
    public int numberOfLanes = 5;
    public float laneWidth = 3f;

    [Header("Spawn Settings")]
    public float spawnZ = 200f;
    public float spawnY = 0.5f;
    public float minDistance = 5f;

    private readonly List<Vector3> recentSpawnPositions = new List<Vector3>();
    private float currentSpawnInterval = 1.5f;
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("ObstacleSpawner: No prefabs assigned.", this);
            enabled = false;
            return;
        }

        // Get initial spawn rate from DifficultyManager
        if (DifficultyManager.Instance != null)
        {
            currentSpawnInterval = DifficultyManager.Instance.GetObstacleSpawnRate();
        }

        StartSpawning();
    }

    void StartSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(SpawnObstacles());
    }

    System.Collections.IEnumerator SpawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnInterval);
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        Vector3 spawnPosition = GetValidLaneSpawnPosition();

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject newObstacle = Instantiate(prefab, spawnPosition, Quaternion.identity);
        
        // Set obstacle speed based on difficulty
        ObstacleMovement obstacleMovement = newObstacle.GetComponent<ObstacleMovement>();
        if (obstacleMovement != null && DifficultyManager.Instance != null)
        {
            obstacleMovement.speed = DifficultyManager.Instance.GetObstacleSpeed();
        }

        if (recentSpawnPositions.Count > 10)
        {
            recentSpawnPositions.RemoveAt(0);
        }
    }

    Vector3 GetValidLaneSpawnPosition()
    {
        const int maxAttempts = 20;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            int laneIndex = Random.Range(0, numberOfLanes);
            float x = CalculateLanePosition(laneIndex);
            Vector3 candidate = new Vector3(x, spawnY, spawnZ);

            bool valid = true;
            foreach (Vector3 pos in recentSpawnPositions)
            {
                if (Vector3.Distance(candidate, pos) < minDistance)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                recentSpawnPositions.Add(candidate);
                return candidate;
            }
        }

        // Fail-safe: spawn anyway (still lane-aligned)
        int fallbackLane = Random.Range(0, numberOfLanes);
        Vector3 fallback = new Vector3(
            CalculateLanePosition(fallbackLane),
            spawnY,
            spawnZ
        );

        recentSpawnPositions.Add(fallback);
        return fallback;
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