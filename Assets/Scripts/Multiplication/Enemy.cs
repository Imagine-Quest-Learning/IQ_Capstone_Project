using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 0.00000001f; //set the speed very low to account for pixel scaling

    private GameObject player;
    private MultiplicationSceneManager sceneManager;
    private float startY;
    private float spawnTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sceneManager = GameObject.FindObjectOfType<MultiplicationSceneManager>();

        startY = transform.position.y;
        spawnTime = Time.time;

        Destroy(this.gameObject, 50f);  //lifespan of the enemy
    }

    void Update()
    {
        if (player == null) return; //If player not found

        Vector2 targetPosition = player.transform.position;
        float followSpeed = 0.05f;

        //Enemy goes towards moving Player
        transform.position = Vector2.Lerp(
            transform.position,
            targetPosition,
            followSpeed * Time.deltaTime
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "magic")
        {
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            sceneManager.PlayerHit();   //Player takes a heart of damage

            Destroy(this.gameObject);
        }
    }
}
