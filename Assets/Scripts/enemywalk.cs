using UnityEngine;

public class enemywalk : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;      // Movement speed of the enemy
    private Rigidbody2D rigid;    // Reference to the Rigidbody2D component

    [Header("Ground Check")]
    public LayerMask groundLayer;         // Layer assigned as "Ground" in the Inspector
    public Transform groundCheck;         // A child object at the enemy's feet for ground detection
    public float checkRadius = 0.2f;      // Radius of the ground check area
    private bool isGrounded;              // Boolean to check if the enemy is on the ground

    void Start()
    {
        // Get the Rigidbody2D component attached to the enemy
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Check if the enemy is on the ground every frame
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void FixedUpdate()
    {
        // Determine the forward direction based on the enemy's scale
        Vector3 front = new Vector3(-1, 0, 0);

        if (transform.localScale.x > 0)
        {
            front = new Vector3(1, 0, 0);
        }

        // Move the enemy horizontally only when it's on the ground
        if (isGrounded)
        {
            rigid.linearVelocity = new Vector2(front.x * speed, rigid.linearVelocity.y);
        }
    }
}
