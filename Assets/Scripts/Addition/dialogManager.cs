using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Description: Controls the dialogue UI.
                 Types out lines one by one, waits for player input(Space).
*/

public class dialogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogPanel;
    public TMP_Text speakerText;
    public TMP_Text bodyText;
    public GameObject continueHint;

    [Header("Typing")]
    public float charInterval = 0.02f;

    public string nextSceneName;

    // References and state
    BasicMovementADD player;
    Dialogue current;
    int index;
    bool isTyping;
    string fullLine;

    void Awake()
    {
        // Find the player once and hide dialogue UI at start
        player = FindFirstObjectByType<BasicMovementADD>();
        if (dialogPanel) dialogPanel.SetActive(false);
        if (continueHint) continueHint.SetActive(false);
    }

    // Called to begin a dialogue
    public void StartDialogue(Dialogue d)
    {
        current = d;
        index = 0;

        dialogPanel.SetActive(true); // Show dialogue box
        if (player) player.SetCanMove(false); // Freeze player movement
        if (speakerText) speakerText.text = d.speaker;

        if (continueHint) continueHint.SetActive(false);
        ShowNext();
    }

    void Update()
    {
        // Do nothing if dialogue panel is not visible
        if (!dialogPanel || !dialogPanel.activeSelf) return;

        // Listen for SPACE or ENTER
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            // If we are still typing characters, skip to full line
            if (isTyping)
            {
                isTyping = false;
                bodyText.text = fullLine;
                if (continueHint) continueHint.SetActive(true);
            }
            else
            {
                // Finished typing this line, go to next
                if (continueHint) continueHint.SetActive(false);
                ShowNext();
            }
        }
    }

    // Move to the next line of dialogue
    void ShowNext()
    {
        if (current == null) return;

        // If no more lines, close dialogue
        if (index >= current.lines.Length)
        {
            EndDialogue();
            return;
        }

        // Get next line and start typing it out
        fullLine = current.lines[index++];
        StopAllCoroutines();
        StartCoroutine(TypeLine(fullLine));
    }

    // Coroutine that types the line letter by letter
    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        bodyText.text = "";

        if (continueHint) continueHint.SetActive(false);

        foreach (char c in line)
        {
            bodyText.text += c; // Add one character
            yield return new WaitForSeconds(charInterval);
            if (!isTyping) yield break; 
        }

        // Finished typing, show continue hint
        isTyping = false;
        if (continueHint) continueHint.SetActive(true);
    }

    // Cleanly end the dialogue and unfreeze player
    void EndDialogue()
    {
        if (continueHint) continueHint.SetActive(false);
        dialogPanel.SetActive(false);
        if (player) player.SetCanMove(true);
        current = null;
        if (nextSceneName == "MainHall")
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else{ return; }
    }
}
