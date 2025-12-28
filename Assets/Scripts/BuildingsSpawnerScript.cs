using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] buildingPrefabs;
    public int buildingCount = 15;

    [Header("X / Y Bounds")]
    public float minX = -30f;
    public float maxX = 30f;
    public float minY = -10f;
    public float maxY = 10f;

    [Tooltip("Minimum distance between buildings (X/Y only)")]
    public float minDistanceBetweenBuildings = 6f;

    [Header("Z Axis Settings")]
    public float startZMin = 20f;
    public float startZMax = 160f;
    public float recycleSpawnZ = 160f;

    [Tooltip("Maximum attempts to find a valid position")]
    public int maxAttempts = 50;

    private List<BuildingMover> activeBuildings = new List<BuildingMover>();

    void Start()
    {
        SpawnInitialBuildings();
    }

    void SpawnInitialBuildings()
    {
        List<Vector2> usedXYPositions = new List<Vector2>();

        for (int i = 0; i < buildingCount; i++)
        {
            float startZ = Random.Range(startZMin, startZMax);
            Vector3 position = GetValidPosition(usedXYPositions, startZ);

            usedXYPositions.Add(new Vector2(position.x, position.y));

            GameObject prefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
            Quaternion rotation = GetRandomRotation();

            GameObject building = Instantiate(prefab, position, rotation, transform);

            BuildingMover mover = building.AddComponent<BuildingMover>();

            // Pass true for initial buildings to skip scale-in
            mover.Initialize(this, isInitialSpawn: true);

            activeBuildings.Add(mover);
        }
    }

    public void RepositionBuilding(BuildingMover building)
    {
        List<Vector2> usedXYPositions = new List<Vector2>();

        foreach (BuildingMover b in activeBuildings)
        {
            if (b != building)
            {
                Vector3 pos = b.transform.position;
                usedXYPositions.Add(new Vector2(pos.x, pos.y));
            }
        }

        Vector3 newPosition = GetValidPosition(usedXYPositions, recycleSpawnZ);
        building.transform.position = newPosition;
        building.transform.rotation = GetRandomRotation();

        // Make recycled buildings scale in
        building.Initialize(this, isInitialSpawn: false);
    }

    Vector3 GetValidPosition(List<Vector2> usedPositions, float zPos)
    {
        Vector2 chosenPos = Vector2.zero;
        bool valid = false;
        int attempts = 0;

        while (!valid && attempts < maxAttempts)
        {
            attempts++;

            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);

            chosenPos = new Vector2(randomX, randomY);

            valid = true;

            foreach (Vector2 used in usedPositions)
            {
                if (Vector2.Distance(chosenPos, used) < minDistanceBetweenBuildings)
                {
                    valid = false;
                    break;
                }
            }
        }

        if (!valid)
        {
            Debug.LogWarning("Could not find valid position. Using fallback.");
        }

        return new Vector3(
            transform.position.x + chosenPos.x,
            transform.position.y + chosenPos.y,
            zPos
        );
    }

    Quaternion GetRandomRotation()
    {
        int step = Random.Range(0, 4);
        return Quaternion.Euler(0f, step * 90f, 0f);
    }
}
