using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float fireRate = 0.001f;
    private float nextFireTime = 0f;

    private MathGameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<MathGameManager>();
    }

    private void Update()
    {
        if (gameManager.isMathActive) return;

        // A and D for rotation
        float rotationInput = 0f
        if (Input.GetKey(KeyCode.A)) rotationInput = 1f;
        if (Input.GetKey(KeyCode.D)) rotationInput = -1f;

        if (rotationInput != 0)
        {
            transform.Rotate(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime);
        }

        // Left and right arrows to fire cannons from sides
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Shoot(leftFirePoint);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Shoot(rightFirePoint);
        }
    }

    private void Shoot(Transform firePoint)
    {
        // Firing cannons logic
        if (Time.time >= nextFireTime)
        {
            GameObject cannon = Instantiate(cannonPrefab, firePoint.position, firePoint.rotation);
            Cannon cannonScript = cannon.GetComponent<Cannon>();

            Vector2 shootDirection = firePoint.right;
            cannonScript.SetDirection(shootDirection);
        }
    }
}
