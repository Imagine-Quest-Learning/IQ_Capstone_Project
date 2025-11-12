using UnityEngine;

public class BatDestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bat"))
        {
            GameObject bat = collision.gameObject;

            //remove bat from SubtractionGameManager List
            if (SubtractionGameManager.Instance != null)
            {
                SubtractionGameManager.Instance.RemoveBatFromList(bat);
            }

            //Destroy the bat
            Destroy(bat);
        }
    }
}
