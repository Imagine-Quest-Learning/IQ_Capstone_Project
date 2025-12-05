using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Description: Acts as a trigger “door” that moves the player to another scene.
                 Plays a fade-out, loads the new scene, then fades back in.
                 Temporarily disables player movement during the transition.
*/
public class ScenePortal : MonoBehaviour
{
    [Header("Scene Transition")]
    public string targetSceneName;
    public float fadeDuration = 0.5f;
    private bool triggered = false;

    [Header("Fade UI")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    void Reset()
    {
        // Automatically try to find a CanvasGroup in the scene
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = FindFirstObjectByType<CanvasGroup>();
        }
    }

    // Trigger detection for player entering the portal
    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        StartCoroutine(DoTransition(other.GetComponent<BasicMovementADD>()));
    }

    // Main sequence: disable movement → fade out → load scene → fade in → re-enable movement
    IEnumerator DoTransition(BasicMovementADD player)
    {
        if (player) player.SetCanMove(false);

        // Fade to black
        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeOut(fadeDuration));

        // Load the next scene
        yield return null;
        SceneManager.LoadScene(targetSceneName);

        // Find the new player in the loaded scene
        var newPlayer = FindFirstObjectByType<BasicMovementADD>();

        // If the new scene has a CanvasGroup for fading, find it again
        if (fadeCanvasGroup == null)
        {
            fadeCanvasGroup = FindFirstObjectByType<CanvasGroup>();
        }

        // Fade in from black
        if (fadeCanvasGroup != null)
            yield return StartCoroutine(FadeIn(fadeDuration));

        if (newPlayer)
            newPlayer.SetCanMove(true);
    }

    // Fade from transparent → opaque (0 → 1)
    private IEnumerator FadeOut(float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        GameObject fadeObj = fadeCanvasGroup.gameObject;
        fadeObj.SetActive(true);

        float t = 0f;
        fadeCanvasGroup.alpha = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(t / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    // Fade from opaque → transparent (1 → 0)
    private IEnumerator FadeIn(float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        GameObject fadeObj = fadeCanvasGroup.gameObject;
        fadeObj.SetActive(true);

        float t = 0f;
        fadeCanvasGroup.alpha = 1f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(t / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeObj.SetActive(false);
    }
}