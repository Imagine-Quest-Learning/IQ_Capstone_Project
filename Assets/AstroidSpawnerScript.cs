using UnityEngine;

public class AstroidSpawnerScript : MonoBehaviour
{
    public GameObject astroid;
    public float spawnRate = 5;
    public float heightOffset = 6;

    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnAstroid(); //avoid waiting forever for the 1st astroid to spawn
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        } else 
        {
            spawnAstroid();
            timer = 0;
        }
    }

    void spawnAstroid()
    {

        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        //Using Random.Range ensures the astroids will spawn anywhere between the lowest/highest points
        Instantiate(astroid, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
