using UnityEngine;

public class Magic : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction = Vector2.right; // Default direction

    void Start()
    {
        Destroy(this.gameObject, 2f); // Lifespan
    }

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized; // Angle it shoots at
    }

    private void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy enemy
            Destroy(gameObject); // Destroy magic
        }
    }
}
