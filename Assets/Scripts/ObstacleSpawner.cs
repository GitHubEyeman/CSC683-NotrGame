using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnRangeX = 6f;
    public float spawnZ = 200f;
    public float minDistance = 5f;

    private readonly List<Vector3> recentSpawnPositions = new List<Vector3>();

    void Start()
    {
        // Safety check
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            Debug.LogError("ObstacleSpawner: No prefabs assigned.", this);
            enabled = false;
            return;
        }

        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnInterval);
    }

    void SpawnObstacle()
    {
        Vector3 spawnPosition = GetValidSpawnPosition();

        GameObject prefab =
            obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Limit stored positions (important for moving obstacles)
        if (recentSpawnPositions.Count > 6)
        {
            recentSpawnPositions.RemoveAt(0);
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 20;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float x = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 candidate = new Vector3(x, 0.5f, spawnZ);

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

        // Fail-safe: spawn anyway to avoid freeze
        Vector3 fallback = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            0.5f,
            spawnZ
        );

        recentSpawnPositions.Add(fallback);
        return fallback;
    }
}
