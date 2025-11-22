using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnDistance = 10f;
    private float spawnTimer;

    private MultiplicationSceneManager sceneManager;

    void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MultiplicationSceneManager>();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && sceneManager.isGameActive)
        {
            SpawnEnemy();
            spawnTimer = spawnRate;
        }
    }

    void SpawnEnemy()
    {
        if (sceneManager.playerHearts > 0 && sceneManager.isGameActive)
        {
            Vector2 spawnPos = GetSpawnPosition();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }

    }

    Vector2 GetSpawnPosition()
    {
        int side = Random.Range(0, 4);
        switch (side)
        {
            // Add enemies from all sides
            case 0: return new Vector2(Random.Range(-spawnDistance, spawnDistance), spawnDistance);
            case 1: return new Vector2(Random.Range(-spawnDistance, spawnDistance), -spawnDistance);
            case 2: return new Vector2(-spawnDistance, Random.Range(-spawnDistance, spawnDistance));
            case 3: return new Vector2(spawnDistance, Random.Range(-spawnDistance, spawnDistance));
            default: return Vector2.zero;
        }
    }
}
