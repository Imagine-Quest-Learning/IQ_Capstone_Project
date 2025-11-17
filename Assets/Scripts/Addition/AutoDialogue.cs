using UnityEngine;

/*
    Description: Automatically starts a dialogue when this scene loads.
                 Looks for the dialogue manager and sends it a Dialogue asset.
*/

public class AutoDialogue : MonoBehaviour
{
    public Dialogue dialogue;
    dialogManager dm;

    void Awake()
    {
        dm = FindFirstObjectByType<dialogManager>();
    }
    void Start()
    {
        // If both manager and dialogue exist, begin the conversation
        if (dm != null && dialogue != null)
            dm.StartDialogue(dialogue);
        else
            Debug.LogWarning("AutoDialogue: Missing dialogManager or Dialogue asset.");
    }
}
