using UnityEngine;
using System.Collections;

public class MultiplicationPlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float fireRate = 0.2f;

    private float nextFireTime = 0f; // Player can shoot magic without cooldown
    private MultiplicationSceneManager sceneManager;

    public Player player;
    private bool hasWand = false; // Initially Player does not have wand
    public bool movementFrozen = false; // Initially Player does not rotate

    private float currentRotation = 0f;
    private const float maxRotation = 80f;

    private IEnumerator Start()
    {
        yield return null;

        sceneManager = FindObjectOfType<MultiplicationSceneManager>();

        // When Player and GameManage does not been set yet 
        if (player == null && GameManager.Instance != null)
        {
            // Get Player with GameInstance
            if (GameManager.Instance.persistentObjects.Length > 1 && GameManager.Instance.persistentObjects[1] != null)
            {
                player = GameManager.Instance.persistentObjects[1].GetComponent<Player>();
            }
        }

        // When Player has not been loaded
        if (player == null)
            player = FindObjectOfType<Player>(true);
        
        // When Player is finally loaded
        if (player != null)
        {
            // Set LeftFirePoint relative to Player
            if (leftFirePoint == null)
            {
                GameObject left = new GameObject("LeftFirePoint");
                left.transform.SetParent(player.transform);
                left.transform.localPosition = new Vector3(-0.069f, 0.015f, 0f);
                leftFirePoint = left.transform;
            }

            // Set RightFirePoint relative to Player
            if (rightFirePoint == null)
            {
                GameObject right = new GameObject("RightFirePoint");
                right.transform.SetParent(player.transform);
                right.transform.localPosition = new Vector3(0.088f, 0.015f, 0f);
                rightFirePoint = right.transform;
            }
        }
    }

    private void Update()
    {
        // Ensure Game has started and Player is loaded
        if (sceneManager != null && sceneManager.isMathActive) return;
        if (player == null) return;

        // Player can no longer rotate
        if (!movementFrozen)
        {
            // Resume normal movement
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            transform.position += movement * Time.deltaTime;
        }

        // If Player has rotated
        if (movementFrozen)
        {
            float rotationInput = 0f;
            if (Input.GetKey(KeyCode.A)) rotationInput = 1f;
            if (Input.GetKey(KeyCode.D)) rotationInput = -1f;

            // Move at an angle
            if (rotationInput != 0)
            {
                currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
                currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

                player.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            }
        }

        if (HasWand)
        {
            // Shoot per direction
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Shoot(leftFirePoint);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Shoot(rightFirePoint);
            }
        }
    }

    public void PickUpWand()
    {
        hasWand = true;
        movementFrozen = true; // Can rotate
    }

    public void RemoveWand()
    {
        hasWand = false;
        movementFrozen = false; // Resume up and down movement
        currentRotation = 0f; // Reset rotation

        if (player != null)
            player.transform.rotation = Quaternion.identity;
    }

    public bool HasWand => hasWand;

    private void Shoot(Transform firePoint)
    {
        if (Time.time >= nextFireTime)
        {
            // Set magic rotation
            Quaternion baseRotation = player.transform.rotation;
            GameObject magic = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            Magic magicScript = magic.GetComponent<Magic>();

            Vector2 shootDirection;

            if (firePoint == leftFirePoint)
            {
                shootDirection = -firePoint.right; // Opposite direction of right
                magic.transform.rotation = baseRotation * Quaternion.Euler(0f, 180f, 0f); // Asset in proper orientation
            }
            else
                shootDirection = firePoint.right;

            magicScript.SetDirection(shootDirection);

            nextFireTime = Time.time + fireRate;
        }
    }


}
