using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 0.00000001f;

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

        Destroy(this.gameObject, 50f);
    }

    void Update()
    {
        if (player == null) return;

        Vector2 targetPosition = player.transform.position;
        float followSpeed = 0.05f;

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
            // Player takes a heart of damage
            sceneManager.PlayerHit();

            Destroy(this.gameObject);
        }
    }
}
