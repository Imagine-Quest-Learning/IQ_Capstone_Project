using UnityEngine;

public class Weapon : MonoBehaviour
{
    private MultiplicationSceneManager sceneManager;
    private MultiplicationPlayerController playerController;

    private void Start()
    {
        sceneManager = GameObject.FindObjectOfType<MultiplicationSceneManager>();
        playerController = GameObject.FindObjectOfType<MultiplicationPlayerController>();
    }

    // Player picks up wand
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController != null)
        {
            playerController.PickUpWand();
            Destroy(gameObject);
        }

        sceneManager.StartGame();
    }
}
