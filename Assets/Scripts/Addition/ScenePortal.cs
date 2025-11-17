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
    public string targetSceneName;
    public float fadeDuration = 0.5f;
    private bool triggered = false;

    // Called when something enters this trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // If already used once, ignore further triggers
        if (triggered) return;

        // Only react to the player
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Start the transition coroutine and pass the player's movement script
        StartCoroutine(DoTransition(other.GetComponent<BasicMovement>()));
    }

    // Handles fade, scene load, and re-enabling player
    IEnumerator DoTransition(BasicMovement player)
    {
        if (player) player.SetCanMove(false);

        // Find a ScreenFader in the scene and fade out
        var fader = FindFirstObjectByType<ScreenFader>();
        if (fader != null)
            yield return fader.FadeOut(fadeDuration);

        // Load the target scene
        yield return null;
        SceneManager.LoadScene(targetSceneName);

        // After load, find the new ScreenFader and fade back in
        fader = FindFirstObjectByType<ScreenFader>();
        if (fader != null)
            yield return fader.FadeIn(fadeDuration);

        var newPlayer = FindFirstObjectByType<BasicMovement>();
        if (newPlayer)
            newPlayer.SetCanMove(true);
    }
}
