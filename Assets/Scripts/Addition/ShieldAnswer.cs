using UnityEngine;
using TMPro;

// Handles displaying the answer number on the soldier's shield
public class ShieldAnswer : MonoBehaviour
{
    public int answer = 0; // The number assigned to this soldier
    public TextMeshProUGUI shieldText;

    // Initialize the shield display at the start
    void Start()
    {
        UpdateShieldDisplay();
    }

    // Updates the text on the shield to show the current answer
    public void UpdateShieldDisplay()
    {
        if (shieldText != null)
        {
            shieldText.text = answer.ToString();
        }
    }
}
