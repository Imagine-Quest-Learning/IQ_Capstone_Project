using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Vector2 direction; // Direction of the cannon's aim

    [Header("References")]
    public Transform FirePoint;           // The position from where the cannonball is fired
    public GameObject CannonBall;         // Prefab for the cannonball
    public GameObject pointPrefab;        // Prefab for trajectory visualization points

    [Header("Trajectory Settings")]
    public int NumberOfPoints = 40;       // Number of trajectory points to display
    public float SpaceBetweenPoints = 0.1f; // Time interval between each trajectory point

    [Header("Fire Settings")]
    public float FireForce = 30f;         // Force applied to the cannonball when fired

    [Header("Gravity Settings")]
    public bool OverrideGlobalGravity = false; // Option to override the global gravity setting
    public float CustomGravityY = -5f;    // Custom gravity value if override is enabled

    private GameObject[] points; // Array to store trajectory visualization points

    void Start()
    {
        // Override global gravity if enabled
        if (OverrideGlobalGravity)
        {
            Physics2D.gravity = new Vector2(0f, CustomGravityY);
        }

        // Initialize the trajectory points and place them at the FirePoint's position
        points = new GameObject[NumberOfPoints];
        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, FirePoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        // Adjust cannon's rotation to follow the mouse cursor
        RotateCannonToMouse();

        // Fire a cannonball when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // Update trajectory visualization points
        for (int i = 0; i < NumberOfPoints; i++)
        {
            float t = i * SpaceBetweenPoints; // Time step for each trajectory point
            points[i].transform.position = CalculateTrajectoryPoint(t);
        }
    }

    // Rotates the cannon to aim towards the mouse cursor
    void RotateCannonToMouse()
    {
        Vector2 cannonPos = transform.position; // Get the cannon's position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Convert mouse position to world space
        direction = mousePos - cannonPos; // Compute the aiming direction
        transform.right = direction; // Rotate the cannon to face the aiming direction
    }

    // Fires a cannonball in the direction the cannon is facing
    void Fire()
    {
        // Instantiate a cannonball at the FirePoint's position and orientation
        GameObject ball = Instantiate(CannonBall, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        if (rb)
        {
            // Apply velocity to the cannonball based on the aiming direction and fire force
            rb.linearVelocity = direction.normalized * FireForce;
        }
    }

    // Calculates the position of a trajectory point based on physics equations
    Vector2 CalculateTrajectoryPoint(float t)
    {
        Vector2 startPos = FirePoint.position; // Initial position of the projectile
        Vector2 initialVelocity = direction.normalized * FireForce; // Initial velocity of the projectile
        return startPos + initialVelocity * t + 0.5f * Physics2D.gravity * (t * t); // Kinematic equation for motion under gravity
    }
}
