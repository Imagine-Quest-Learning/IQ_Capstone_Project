using UnityEngine;

public class MultiplicationPlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private GameObject magicPrefab;
    [SerializeField] private Transform leftFirePoint;
    [SerializeField] private Transform rightFirePoint;
    [SerializeField] private float fireRate = 0.2f;

    private float nextFireTime = 0f;
    private MultiplicationSceneManager sceneManager;

    public Player player;
    private bool hasWand = false;
    public bool movementFrozen = false;

    private float currentRotation = 0f;
    private const float maxRotation = 80f;

    private void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MultiplicationSceneManager>();
        if (player == null)
            player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (sceneManager != null && sceneManager.isMathActive) return;
        if (player == null) return;

        if (!movementFrozen)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            transform.position += movement * Time.deltaTime;
        }

        if (movementFrozen)
        {
            float rotationInput = 0f;
            if (Input.GetKey(KeyCode.A)) rotationInput = 1f;
            if (Input.GetKey(KeyCode.D)) rotationInput = -1f;

            if (rotationInput != 0)
            {
                currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
                currentRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

                player.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
            }
        }

        if (HasWand)
        {
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
        movementFrozen = true;
        Debug.Log("Wand picked up! Movement frozen.");
    }

    public bool HasWand => hasWand;

    private void Shoot(Transform firePoint)
    {
        if (Time.time >= nextFireTime)
        {
            GameObject magic = Instantiate(magicPrefab, firePoint.position, firePoint.rotation);
            Magic magicScript = magic.GetComponent<Magic>();

            Vector2 shootDirection = firePoint.right;
            magicScript.SetDirection(shootDirection);
        }
    }

}
