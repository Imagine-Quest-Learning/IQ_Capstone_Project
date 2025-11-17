using System.Collections;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject bat; //assign bat prefab in unity inspector
    public float spawnRate = 8f; //how long until next bat spawns
    public float fatestSpawnRate = 4f; 
    public float heightOffset = 0.5f;
    public float globalBatSpeed = 0.4f;
    public bool spawningActive = false;
    public SubtractionGameManager gameManager;

    private float lastSpawnTime = 0f;
    private float instantSpawnCooldown = 1f;

    void Start()
    {   
        //Start spawning automatically if spawningActive is checked
        if(spawningActive)
        {
            StartCoroutine(SpawnBats());
        }
    }
    public void StartSpawning()
    {
        if (!spawningActive)
        {
            spawningActive = true;
            StartCoroutine(SpawnBats());
        }
    }

    public void StopSpawning()
    {
        spawningActive = false;

        //destroy all existing bats
        GameObject[] bats = GameObject.FindGameObjectsWithTag("Bat");
        foreach (GameObject bat in bats)
        {
            Destroy(bat);
        }
    }

    public void DecreaseSpawnRate()
    {
        //spawn bats faster if the current spawn rate is slower than the fastest possible spawn rate
        if (spawnRate > fatestSpawnRate)
        {
            spawnRate = spawnRate - 0.5f;
        }
        Debug.Log($"Spawn Rate = {spawnRate}");
    }

    public void SpawnSingleBat()
    {
        if (Time.time - lastSpawnTime < instantSpawnCooldown) return;

        if (!spawningActive) return;

        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        // Have bats rotated to look like they're flying in
        Quaternion batRotation = Quaternion.Euler(0f, 0f, -66.202f);

        //Using Random.Range ensures bats spawn anywhere between highest and lowest points
        GameObject newBat = Instantiate(bat,
                new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0),
                batRotation);

        //Set speed of new bat to global speed
        Bat batScript = newBat.GetComponent<Bat>();
        if (batScript != null)
        {
            batScript.setMoveSpeed(globalBatSpeed);
            Debug.Log($"Bat Speed: {globalBatSpeed}");
        }

        //register new bat with game manager
        if (gameManager != null)
        {
            gameManager.RegisterBat(newBat);
        }

        lastSpawnTime = Time.time;

        Debug.Log($"Bat Spawned!");
    }
    
    private IEnumerator SpawnBats()
    {
        while (spawningActive)
        {
            SpawnSingleBat();

            //wait for next spawnRate to spawn the next bat
            yield return new WaitForSeconds(spawnRate); 
        }
    }

}
