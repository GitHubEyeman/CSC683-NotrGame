using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // Array of enemy prefabs to choose from
    public float spawnRadius = 10f;    // Radius around the spawner where enemies will spawn
    public float minSpawnDistance = 2f; // Minimum distance between spawns to avoid overlap
    public float spawnInterval = 5f;    // Time interval between spawns
    public float maxSpawnHeight = 10f;
    public Transform spawnerTransform;  // Reference to the spawner's transform

    private Vector3 lastSpawnPosition;  // To keep track of the last spawn position

    void Start()
    {
        lastSpawnPosition = spawnerTransform.position;  // Initialize last spawn position to spawner's position
        StartCoroutine(SpawnEnemy());  // Start the enemy spawning coroutine
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // Wait for the specified interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);

            // Randomly pick an enemy prefab from the array
            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Find a spawn position within the given radius, making sure it's not too close to the last spawn
            Vector3 spawnPosition = GetRandomSpawnPosition();

            // Instantiate the enemy prefab at the spawn position
            GameObject spawnedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);

            // If the Y position of the spawned enemy is above 15, start moving it down
            if (spawnedEnemy.transform.position.y >= maxSpawnHeight)
            {
                StartCoroutine(MoveDownSmoothly(spawnedEnemy));
            }

            
        }
    }

    // Function to find a random spawn position within a radius that avoids overlap
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        float distanceToLastSpawn;

        // Repeat until we find a spawn position far enough from the last spawn
        do
        {
            // Generate a random position within the spawn radius
            spawnPosition = new Vector3(
                spawnerTransform.position.x + Random.Range(-spawnRadius, spawnRadius),
                spawnerTransform.position.y,
                spawnerTransform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            // Calculate the distance to the last spawn position
            distanceToLastSpawn = Vector3.Distance(spawnPosition, lastSpawnPosition);

        } while (distanceToLastSpawn < minSpawnDistance);  // Ensure it's not too close

        lastSpawnPosition = spawnPosition;  // Update the last spawn position
        return spawnPosition;  // Return the valid spawn position
    }

    // Coroutine to smoothly move the enemy down to a Y position below 10
    IEnumerator MoveDownSmoothly(GameObject enemy)
    {
        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, 9f, startPos.z);  // Target position at y = 9
        float elapsedTime = 0f;
        float duration = 1f;  // Duration for the smooth movement

        while (elapsedTime < duration)
        {
            enemy.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = targetPos;  // Ensure the enemy ends exactly at target position
    }
}
