using System.Collections;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    public GameObject enemyPrefab; // Reference to the enemy prefab
    public Transform spawnPoint; // Reference to the spawn point


    public float startTime = 0f; // Time before the first enemy spawns
    public float endTime = 60f; // Time after which no more enemies will spawn
    public float spawnIntervalStart = 10f; // Time interval between enemy spawns
    public float spawnIntervalEnd = 1f; // Time interval between enemy spawns at the end time


    private Coroutine spawnCoroutine; // Reference to the spawning coroutine


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCoroutine(float GameEndTime)
    {
        endTime = GameEndTime;

        float duration = endTime - startTime;

        float elapsed = 0f;
        float startInterval =  spawnIntervalStart;
        float endInterval = spawnIntervalEnd;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            float spawnInterval = Mathf.Lerp(startInterval, endInterval, t);

            SpawnObject();

            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }

        spawnCoroutine = null;
    }

    void SpawnObject()
    {
        if (enemyPrefab != null && spawnPoint != null)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    public void startSpawning(float GameEndTime)
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnCoroutine(GameEndTime));
        }
    }
    public void stopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
