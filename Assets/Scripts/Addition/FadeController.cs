using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Coroutine for fading in (transitioning from transparent to opaque)
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        // Ensure the fadeImage is fully opaque after fading in
        SetAlpha(1f);
    }

    // Coroutine for fading out (transitioning from opaque to transparent)
    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            SetAlpha(alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the fadeImage is fully transparent after fading out
        SetAlpha(0f);
    }

    // Helper method to update the alpha value of the fadeImage
    void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }

    // Public method to initiate a scene transition with a fade effect
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    // Coroutine that handles the complete scene transition process
    private IEnumerator Transition(string sceneName)
    {
        yield return StartCoroutine(FadeIn());
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(FadeOut());
    }
}
