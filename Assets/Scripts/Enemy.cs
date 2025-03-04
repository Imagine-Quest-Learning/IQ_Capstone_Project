using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    private GameObject player;
    private MathGameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindObjectOfType<MathGameManager>();

        // Enemy lifespan
        Destroy(this.gameObject, 20f);
    }

    void Update()
    {
        // Move towards middle (player) and angle properly
        Vector2 targetPosition = Vector2.zero;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 180);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "cannon")
        {
            // Die when hit by fired cannon
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            // Player takes a heart of damage
            gameManager.PlayerHit();

            Destroy(this.gameObject);
        }
    }
}
