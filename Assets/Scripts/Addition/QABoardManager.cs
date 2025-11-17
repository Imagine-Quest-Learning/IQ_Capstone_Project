using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Manages question generation, answer checking, and UI interaction in the gamepage
public class QABoardManager : MonoBehaviour
{
    [Header("Question Settings")]
    public int a = 2;
    public int b = 3;
    public TextMeshProUGUI questionText;

    [Header("Dialog UI")]
    public GameObject dialogPanel; // Panel that shows "Correct"/"Wrong"
    public TextMeshProUGUI dialogText; // Text inside the dialog panel

    [Header("Sub-Text for Correct Answer")]
    public TextMeshProUGUI dialogSubText; // Text field for showing the correct answer

    [Header("Restart Button")]
    public Button restartButton;

    [Header("Menu Button")]
    public Button BackMenuButton; // Button to return to the start page

    // Singleton pattern to allow easy access to this script
    public static QABoardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        GenerateRandomQuestion();      // Randomly create the math question
        AssignAnswersToSoldiers();     // Assign answers to all soldier shields

        if (dialogPanel != null)
            dialogPanel.SetActive(false);

        // Hide sub-text by default
        if (dialogSubText != null)
            dialogSubText.gameObject.SetActive(false);

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
            restartButton.onClick.AddListener(RestartGame);
        }

        if (BackMenuButton != null)
        {
            BackMenuButton.gameObject.SetActive(false);
            BackMenuButton.onClick.AddListener(BackToStartPage);
        }
    }

    // Randomly generate values for a and b, and display the question
    private void GenerateRandomQuestion()
    {
        a = Random.Range(0, 10);
        b = Random.Range(0, 10);

        if (questionText != null)
            questionText.text = $"{a} + {b} = ?";
    }

    // Assign answers to all soldiers, with one having the correct result
    private void AssignAnswersToSoldiers()
    {
#if UNITY_2023_1_OR_NEWER
        ShieldAnswer[] allSoldiers =
            FindObjectsByType<ShieldAnswer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
#else
        ShieldAnswer[] allSoldiers = FindObjectsOfType<ShieldAnswer>();
#endif

        if (allSoldiers.Length == 0) return;

        int correctIndex = Random.Range(0, allSoldiers.Length);

        for (int i = 0; i < allSoldiers.Length; i++)
        {
            if (i == correctIndex)
            {
                allSoldiers[i].answer = a + b; // Assign correct answer
            }
            else
            {
                int wrongAnswer;
                do
                {
                    wrongAnswer = Random.Range(0, 20);
                } while (wrongAnswer == (a + b)); // Ensure wrong answer is not equal to correct
                allSoldiers[i].answer = wrongAnswer;
            }
            // Update the number on the shield UI
            allSoldiers[i].UpdateShieldDisplay();
        }
    }

    // Property to get the correct answer
    public int CorrectAnswer => a + b;

    // Called when a soldier is hit; checks if the answer is correct
    public void CheckAnswer(int soldierAnswer)
    {
        bool isCorrect = (soldierAnswer == (a + b));

        Color resultColor = isCorrect ? Color.Lerp(Color.green, Color.gray, 0.2f)
                                      : Color.Lerp(Color.red, Color.gray, 0.2f);
        float fontSize = isCorrect ? 50 : 45;
        string message = isCorrect ? "Correct!" : "Wrong!";

        // If correct, show sub-text with the actual answer. Otherwise hide it.
        if (dialogSubText != null)
        {
            if (isCorrect)
            {
                dialogSubText.gameObject.SetActive(true);
                dialogSubText.text = $"Answer is: {a + b}, You got the Key!";
            }
            else
            {
                dialogSubText.gameObject.SetActive(false);
            }
        }

        ShowDialog(message, resultColor, fontSize, isCorrect);
    }

    // Display result dialog and optionally freeze the game
    public void ShowDialog(string message, Color textColor, float fontSize, bool freezeGame)
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(true);

        if (dialogText != null)
        {
            dialogText.text = message;
            dialogText.color = textColor;
            dialogText.fontSize = fontSize;
        }

        CannonController.canShoot = false;

        if (freezeGame)
        {
            Time.timeScale = 0f;
            if (restartButton != null)
                StartCoroutine(ShowRestartButtonAfterDelay(2f)); // Show buttons after delay
        }
        else
        {
            Invoke(nameof(HideDialog), 2f);
        }
    }

    // Hide the dialog panel and allow shooting again
    public void HideDialog()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
        CannonController.canShoot = true;
    }

    // Restart the current game scene
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Go back to the start page scene
    private void BackToStartPage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("startpage");
    }

    // Delay showing the Restart and Menu buttons after correct answer
    private System.Collections.IEnumerator ShowRestartButtonAfterDelay(float delay)
    {
        float timePassed = 0f;
        while (timePassed < delay)
        {
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (restartButton != null)
            restartButton.gameObject.SetActive(true);

        if (BackMenuButton != null)
            BackMenuButton.gameObject.SetActive(true);
    }
}
