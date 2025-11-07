using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
    File: SceneChanger

    Description: Detect when a player collides with a trigger
                 and change the scene with a fade transition
    
    Written By: Ethan Tang [2025]
*/
public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad;
    public Animator fadeAnimation; //type of scene transition
    public float fadeTime = .5f;
    public Vector2 newPlayerPosition; //where player appears once new scene is loaded
    private Transform player; //hold reference to player's Transform so we can move player after fade

    //Needed for testing
    public bool disableSceneLoad = false;
    public bool disableFadeAnimation = false;
    
    /*
        OnTriggerEnter2D
        - Check if player runs into trigger
        - Load new scene IF room has NOT been complete
    */
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Check if Room is Complete
            if (GameManager.Instance != null && GameManager.Instance.IsRoomComplete(sceneToLoad))
            {
                Debug.Log($"Room '{sceneToLoad}' is already complete - entry disabled");
                return;
            }

            player = collision.transform;

            if (!disableFadeAnimation && fadeAnimation != null)
            {
                fadeAnimation.Play("FadeToWhite");
            }

            StartCoroutine(DelayFade());
        }
    }

    /*
        DelayFade()
        - Coroutine to wait for fade to finish and move player to new position
        - Loads new scene for player
    */
    IEnumerator DelayFade()
    {
        // Reset player rotation
        if (player != null)
        {
            player.rotation = Quaternion.identity;
        }

        yield return new WaitForSeconds(fadeTime);
        player.position = newPlayerPosition;

        if (!disableSceneLoad)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
