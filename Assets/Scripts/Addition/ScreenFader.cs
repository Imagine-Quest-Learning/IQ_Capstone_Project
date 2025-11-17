using System.Collections;
using UnityEngine;

/*
    
    Description: Controls the screen fade UI.
                 Fades from clear to black and from black to clear
                 using a CanvasGroup alpha value.

*/
public class ScreenFader : MonoBehaviour
{
    // CanvasGroup used to control fade transparency
    [SerializeField] CanvasGroup canvasGroup;
    public float defaultDuration = 0.5f;

    // Try to auto-find a CanvasGroup on this object or its children
    void Reset()
    {
        canvasGroup = GetComponentInChildren<CanvasGroup>(true);
    }

    public IEnumerator FadeOut(float duration = -1f)
    {
        // Use default duration if caller did not specify one
        if (duration < 0) duration = defaultDuration;

        // Make sure the fader object is visible
        gameObject.SetActive(true);

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // progress over real time
            canvasGroup.alpha = Mathf.Clamp01(t / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    // Fade from opaque → transparent (1 → 0)
    public IEnumerator FadeIn(float duration = -1f)
    {
        if (duration < 0) duration = defaultDuration;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(t / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
