using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MultiplicationSceneManager : MonoBehaviour
{
    public GameObject mathPanel;
    public Text questionText;
    public InputField answerInput;
    public Text scoreText;

    public GameObject instructionsPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public GameObject wandPrefab;

    public Image[] heartSprites;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private bool gameActive = false;
    public bool isMathActive = false;
    public int minMathInterval = 5;
    public int maxMathInterval = 12;
    private int questions = 0;
    public int playerHearts = 3;
    private int correctAnswer;

    private Coroutine mathTimerCoroutine;

    void Start()
    {
        instructionsPanel.SetActive(true);
        mathPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        UpdateUI();

        answerInput.onEndEdit.AddListener(delegate { CheckAnswerOnEnter(); });
    }

    public void StartGame()
    {
        if (!isGameActive)
        {
            instructionsPanel.SetActive(false);
            gameActive = true;
            questions = 0;
            playerHearts = 3;
            UpdateUI();

            if (mathTimerCoroutine != null)
                StopCoroutine(mathTimerCoroutine);

            mathTimerCoroutine = StartCoroutine(MathQuestionTimer());
        }
    }


    IEnumerator MathQuestionTimer()
    {
        while (playerHearts > 0 && isGameActive)
        {
            // Show question after a random amount of time
            yield return new WaitForSeconds(Random.Range(minMathInterval, maxMathInterval));

            if (playerHearts > 0 && !isMathActive && isGameActive)
            {
                ShowMathQuestion();
            }
        }
    }

    void ShowMathQuestion()
    {
        if (isMathActive) return;

        isMathActive = true;
        mathPanel.SetActive(true);
        GenerateMathProblem();
        answerInput.text = "";
        answerInput.Select();
    }


    void GenerateMathProblem()
    {
        // Generate problem with these parameters (up to 12x12)
        int a = Random.Range(1, 12);
        int b = Random.Range(1, 12);
        correctAnswer = a * b;

        // Show question
        questionText.text = a + " x " + b + "?";
    }
    
    public void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                questions += 1;
                ResetMathGame();

                if (questions == 10)
                {
                    GameOver();
                    winPanel.SetActive(true);
                }
            }
            else
            {
                playerHearts--;
                UpdateUI();

                if (playerHearts <= 0)
                {
                    GameOver();
                    RespawnWand();
                    losePanel.SetActive(true);
                }
            }
        }
        // Reset question input field after submitting
        answerInput.text = "";
    }

    void CheckAnswerOnEnter()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }


    public void PlayerHit()
    {
        // Player loses heart when hit by enemy
        playerHearts--;

        if (playerHearts <= 0)
        {
            GameOver();
        }
        UpdateUI();
    }

    void ResetMathGame()
    {
        Time.timeScale = 1;
        answerInput.text = "";
        mathPanel.SetActive(false);
        isMathActive = false;
        UpdateUI();
    }

    void GameOver()
    {
        ClearAllEnemies();
        mathPanel.SetActive(false);
        isMathActive = false;
        gameActive = false;
        FindObjectOfType<MultiplicationPlayerController>().RemoveWand();
    }

    void UpdateUI()
    {
        scoreText.text = $"{questions}" + " / 10";

        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (i < playerHearts)
                heartSprites[i].sprite = fullHeart;
            else
                heartSprites[i].sprite = emptyHeart;
        }
    }

    public void MainMenu()
    {
        ResetMathGame();
        SceneManager.LoadSceneAsync(1);
    }

    public void Restart()
    {
        ResetMathGame();
        SceneManager.LoadSceneAsync(0);
    }

    public void Close()
    {
        instructionsPanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }

    void RespawnWand()
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
        Instantiate(wandPrefab, spawnPosition, Quaternion.identity);
    }

    public void ClearAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    public bool isGameActive => gameActive;

}
