using UnityEngine;
using System.Collections;

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

    private IEnumerator Start()
    {
        yield return null;

        sceneManager = FindObjectOfType<MultiplicationSceneManager>();

        if (player == null && GameManager.Instance != null)
        {
            if (GameManager.Instance.persistentObjects.Length > 1 && GameManager.Instance.persistentObjects[1] != null)
            {
                player = GameManager.Instance.persistentObjects[1].GetComponent<Player>();
            }
        }

        if (player == null)
            player = FindObjectOfType<Player>(true);

        if (player != null)
        {
            if (leftFirePoint == null)
            {
                GameObject left = new GameObject("LeftFirePoint");
                left.transform.SetParent(player.transform);
                left.transform.localPosition = new Vector3(-0.069f, 0.015f, 0f);
                leftFirePoint = left.transform;
            }

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

            Vector2 shootDirection;

            if (firePoint == leftFirePoint)
                shootDirection = -firePoint.right;
            else
                shootDirection = firePoint.right;

            magicScript.SetDirection(shootDirection);

            nextFireTime = Time.time + fireRate;
        }
    }


}
