using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/*

    Description: Simple timer that waits a few seconds
                 and then loads another scene.

*/
public class DialogAutoReturnSimple : MonoBehaviour
{
    [SerializeField] string targetSceneName;
    [SerializeField] float delaySeconds;
    bool started;

    void OnEnable()
    {
        if (started) return;
        started = true;
        StartCoroutine(JumpAfterDelay());
    }

    // Wait, then change scene
    IEnumerator JumpAfterDelay()
    {
        yield return new WaitForSecondsRealtime(delaySeconds);

        // Make sure game speed is normal
        if (Time.timeScale != 1f) Time.timeScale = 1f;

        SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
    }
}
