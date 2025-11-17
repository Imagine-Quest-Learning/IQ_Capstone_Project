using UnityEngine;

// Script to control an enemy walking back and forth between two points
public class enemywalk : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;

        rb.gravityScale = 0; // Disable gravity to keep enemy on same Y level
    }

    void Update()
    {
        Vector2 movement = currentPoint.position - transform.position;

        // If close enough to the current point, stop and switch direction
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            rb.linearVelocity = Vector2.zero;

            // Change direction: toggle between two points
            if (currentPoint == pointB.transform)
                currentPoint = pointA.transform;
            else
                currentPoint = pointB.transform;

            flip(); // Flip the enemy's facing direction
        }
        else
        {
            // Move horizontally toward the current target point
            rb.linearVelocity = new Vector2(speed * Mathf.Sign(movement.x), 0);
        }
    }

    // Flips the enemy horizontally when switching direction
    void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x = -localScale.x;
        transform.localScale = localScale;
    }

    // Visual aid in the editor: draw spheres at patrol points and a line between them
    void OnDrawGizmos()
    {
        if (pointA != null) Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        if (pointB != null) Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        if (pointA != null && pointB != null) Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
