using UnityEngine;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour
{
    public Text[] instructions;
    public SubtractionGameManager gameManager;
    private int currentIndex = 0;

    private void OnEnable()
    {
        foreach (Text line in instructions)
        {
            line.gameObject.SetActive(false);
        }
        if (instructions.Length > 0)
        {
            instructions[0].gameObject.SetActive(true);
        }

        currentIndex = 0;
    }

    void Update()
    {
        //Check for Enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShowNextLine();
        }
    }

    private void ShowNextLine()
    {
        if (instructions == null || instructions.Length == 0) return;

        if (currentIndex < instructions.Length - 1)
        {
            currentIndex++;
            instructions[currentIndex].gameObject.SetActive(true);
        }
        else
        {
            if (gameManager != null)
            {
                Debug.Log("Start Subtraction Game");
                gameManager.StartSubtractionGame();
            }
            gameObject.SetActive(false);
        }
    }
    
}
