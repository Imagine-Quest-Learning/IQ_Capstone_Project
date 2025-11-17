using UnityEngine;

[System.Serializable]
// Mark this class so it can be edited in the Inspector
public class Dialogue
{
    public string speaker;
    [TextArea(2, 5)]
    public string[] lines;
}