using UnityEngine;

public class Bat : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        //Move bat to the right
        transform.position = transform.position + (Vector3.right * moveSpeed) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the bat collides with a barrel
        if(collision.CompareTag("Barrel"))
        {
            //destroy barrel
            SubtractionGameManager.Instance.barrelManager.DestroyBarrel(collision.gameObject);

            //remove bat from SubtractionGameManager List
            if (SubtractionGameManager.Instance != null)
            {
                SubtractionGameManager.Instance.RemoveBatFromList(gameObject);
            }

            //destroy bat itself
            Destroy(gameObject);
        }
    }

    public void setMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
