using System.Collections.Generic;
using UnityEngine;

public class BuildingsSpawnerScript : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array of prefabs to spawn
    public int numberOfSpawns = 10; // How many prefabs to spawn
    public float spawnRangeX = 30f; // Range for X axis
    public float spawnRangeY = 15f; // Range for Y axis
    public float spawnInterval = 2f;
    public float minDistance = 5f;
    private List<Vector3> spawnPositions = new List<Vector3>();

    private float initialPosX;
    private float initialPosY;
    private float initialPosZ;

    void Start()
    {
        initialPosX = transform.position.x;
        initialPosY = transform.position.y;
        initialPosZ = transform.position.z;
        InvokeRepeating("SpawnPrefabs", 0f, spawnInterval);
    }

    void SpawnPrefabs()
    {
        for (int i = 0; i < numberOfSpawns; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();

            GameObject prefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];

            GameObject building = Instantiate(prefab, spawnPosition, Quaternion.identity);
            building.transform.SetParent(transform);
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        bool positionIsValid = false;

        // Keep trying until a valid position is found
        do
        {
            // Random position within the specified ranges relative to the object's position
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            float randomY = Random.Range(-spawnRangeY, spawnRangeY);

            spawnPosition = new Vector3(randomX + initialPosX, randomY + initialPosY, initialPosZ);

            positionIsValid = true;

            // Check if this position is too close to any existing spawn positions
            foreach (Vector3 existingPosition in spawnPositions)
            {
                if (Vector3.Distance(spawnPosition, existingPosition) < minDistance)
                {
                    positionIsValid = false; // Too close, try again
                    break;
                }
            }
        }
        while (!positionIsValid); // Continue until a valid position is found

        // Add the valid position to the list
        spawnPositions.Add(spawnPosition);

        return spawnPosition;
    }
}
