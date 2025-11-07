using System.Collections;
using UnityEngine;

public class BatSpawner : MonoBehaviour
{
    public GameObject bat; //assign bat prefab in unity inspector
    public float spawnRate = 3f; //how long until next bat spawns
    public float heightOffset = 0.5f;
    public float globalBatSpeed = 0.5f;
    public float fastestSpeed = 1f;
    public float slowestSpeed = 0.1f;
    public bool spawningActive = false;

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
    }

    private IEnumerator SpawnBats()
    {
        while (spawningActive)
        {
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
            Debug.Log($"Bat Spawned!");

            //wait for next spawnRate to spawn the next bat
            yield return new WaitForSeconds(spawnRate); 
        }
    }

}
