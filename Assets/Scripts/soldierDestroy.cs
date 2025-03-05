using UnityEngine;

public class soldierDestroy : MonoBehaviour
{
    public int maxHP = 1;
    private int currentHP;

    private GameObject boomPrefab;

    private void Start()
    {
        currentHP = maxHP;
        boomPrefab = Resources.Load<GameObject>("Boom1");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentHP -= (int)(collision.relativeVelocity.magnitude * 8);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

}
