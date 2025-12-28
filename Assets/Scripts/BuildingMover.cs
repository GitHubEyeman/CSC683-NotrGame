using System.Collections;
using UnityEngine;

public class BuildingMover : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 50f;
    public float recycleZ = -25f;

    [Header("Scale In")]
    public float scaleDuration = 15.5f;

    private BuildingSpawner spawner;
    private Vector3 originalScale;
    private bool skipScaleIn = false;

    public void Initialize(BuildingSpawner buildingSpawner, bool isInitialSpawn = false)
    {
        spawner = buildingSpawner;
        originalScale = transform.localScale;

        if (isInitialSpawn)
        {
            skipScaleIn = true;
            transform.localScale = originalScale;
        }
        else
        {
            skipScaleIn = false;
            StopAllCoroutines();
            StartCoroutine(ScaleIn());
        }
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;

        if (transform.position.z <= recycleZ)
        {
            spawner.RepositionBuilding(this);
        }
    }

    private IEnumerator ScaleIn()
    {
        float t = 0f;
        transform.localScale = Vector3.zero;

        while (t < scaleDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / scaleDuration);

            // Ease-out cubic: fast start, smooth finish
            float eased = 1f - Mathf.Pow(1f - normalizedTime, 3f);

            transform.localScale = originalScale * eased;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
