using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private Vector2 direction;

    void Start()
    {
        // Lifespan of fired cannon
        Destroy(this.gameObject, 5f);
    }

    public void SetDirection(Vector2 newDirection)
    {
        // Fire based on player direction
        direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)direction * bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy enemy
            Destroy(gameObject); // Destroy bullet
        }
    }
}
