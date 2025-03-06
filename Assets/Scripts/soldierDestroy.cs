using UnityEngine;

public class soldierDestroy : MonoBehaviour
{
    public int maxHP = 1;
    private int currentHP;

    private GameObject boomPrefab;

    private void Start()
    {
        // Initialize current health points
        currentHP = maxHP;

        // Load the explosion prefab
        boomPrefab = Resources.Load<GameObject>("Boom1");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calculate damage based on the collision's relative velocity and reduce health
        currentHP -= (int)(collision.relativeVelocity.magnitude * 8);

        // If health points reach 0 or below, destroy the soldier object
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
