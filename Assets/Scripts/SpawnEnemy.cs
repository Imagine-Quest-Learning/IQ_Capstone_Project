using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnDistance = 10f;
    private float spawnTimer;

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
            spawnTimer = spawnRate;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = GetSpawnPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetSpawnPosition()
    {
        // Randomly choose spawn side of enemy
        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0: return new Vector2(Random.Range(-spawnDistance, spawnDistance), spawnDistance); // Top
            case 1: return new Vector2(Random.Range(-spawnDistance, spawnDistance), -spawnDistance); // Bottom
            case 2: return new Vector2(-spawnDistance, Random.Range(-spawnDistance, spawnDistance)); // Left
            case 3: return new Vector2(spawnDistance, Random.Range(-spawnDistance, spawnDistance)); // Right
            default: return Vector2.zero;
        }
    }
}
