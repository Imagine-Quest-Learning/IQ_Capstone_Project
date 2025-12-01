using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*
    Description: Simple timer that waits a few seconds
                 and then loads another scene,
                 but first repeats the current scene a few times.
*/
public class DialogAutoReturnSimple : MonoBehaviour
{
    [SerializeField] string targetSceneName;
    [SerializeField] float delaySeconds = 2f;

    // Number of times to repeat the current screen (excluding the first time)
    [SerializeField] int repeatCount = 3;

    bool started;

    // Shared counter across scene reloads (does not reset when scene is reloaded)
    static int timesShown = 0;

    void OnEnable()
    {
        if (started) return;
        started = true;
        StartCoroutine(JumpAfterDelay());
    }

    IEnumerator JumpAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delaySeconds);

        // Ensure normal game speed
        if (Time.timeScale != 1f) Time.timeScale = 1f;

        // Increase number of times this scene has been shown
        timesShown++;

        if (timesShown <= repeatCount)
        {
            // Not the last time yet: reload the current scene
            string currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene, LoadSceneMode.Single);
        }
        else
        {
            // Finished (first time + repeatCount times): reset counter and go to target scene
            timesShown = 0;
            SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
        }
    }
}
