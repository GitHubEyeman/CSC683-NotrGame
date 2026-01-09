using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour  // CHANGE THIS LINE
{
    public GameObject[] enemyPrefabs;  // Array of enemy prefabs to choose from
    public float spawnRadius = 10f;    // Radius around the spawner where enemies will spawn
    public float minSpawnDistance = 2f; // Minimum distance between spawns to avoid overlap
    public float spawnInterval = 5f;    // Time interval between spawns
    public float maxSpawnHeight = 10f;
    public Transform spawnerTransform;  // Reference to the spawner's transform

    private Vector3 lastSpawnPosition;  // To keep track of the last spawn position
    private Coroutine spawnCoroutine;

    void Start()
    {
        lastSpawnPosition = spawnerTransform.position;  // Initialize last spawn position to spawner's position
    }
    
    public void StartSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
            
        spawnCoroutine = StartCoroutine(SpawnEnemy());
    }
    
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            // Check if game is running and not paused
            if (GameManager.Instance == null || !GameManager.Instance.isGameRunning || GameManager.Instance.isGamePaused)
            {
                yield return new WaitUntil(() => GameManager.Instance != null && 
                                                 GameManager.Instance.isGameRunning && 
                                                 !GameManager.Instance.isGamePaused);
            }
            
            yield return new WaitForSeconds(spawnInterval);

            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject spawnedEnemy = Instantiate(enemy, spawnPosition, Quaternion.identity);

            if (spawnedEnemy.transform.position.y >= maxSpawnHeight)
            {
                StartCoroutine(MoveDownSmoothly(spawnedEnemy));
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition;
        float distanceToLastSpawn;

        do
        {
            spawnPosition = new Vector3(
                spawnerTransform.position.x + Random.Range(-spawnRadius, spawnRadius),
                spawnerTransform.position.y,
                spawnerTransform.position.z + Random.Range(-spawnRadius, spawnRadius)
            );

            distanceToLastSpawn = Vector3.Distance(spawnPosition, lastSpawnPosition);

        } while (distanceToLastSpawn < minSpawnDistance);

        lastSpawnPosition = spawnPosition;
        return spawnPosition;
    }

    IEnumerator MoveDownSmoothly(GameObject enemy)
    {
        Vector3 startPos = enemy.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, 9f, startPos.z);
        float elapsedTime = 0f;
        float duration = 1f;

        while (elapsedTime < duration)
        {
            if (GameManager.Instance != null && GameManager.Instance.isGamePaused)
            {
                yield return new WaitUntil(() => !GameManager.Instance.isGamePaused);
            }
            
            enemy.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = targetPos;
    }
}