using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MathGameManager : MonoBehaviour
{
    public GameObject mathPanel;
    public Text questionText;
    public InputField answerInput;
    public Text scoreText;

    public Image[] heartSprites;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public bool isMathActive = false;
    public int minMathInterval = 5;
    public int maxMathInterval = 12;
    private int score = 0;
    private int playerHearts = 3;
    private int correctAnswer

    void Start()
    {
        // Make sure the math panel is initially not visible
        mathPanel.SetActive(false);
        StartCoroutine(ScoreIncrement());
        StartCoroutine(MathQuestionTimer());
        UpdateUI();
    }

    IEnumerator MathQuestionTimer()
    {
        while (playerHearts > 0)
        {
            // Show question after a random amount of time
            yield return new WaitForSeconds(Random.Range(minMathInterval, maxMathInterval));

            if (playerHearts > 0 && !isMathActive)
            {
                ShowMathQuestion();
            }
        }
    }

    void ShowMathQuestion()
    {
        isMathActive = true;
        mathPanel.SetActive(true);
        GenerateMathProblem();
    }

    void GenerateMathProblem()
    {
        // Generate problem with these parameters (first number from 1-19)
        int a = Random.Range(1, 20);
        int b = Random.Range(1, a);
        correctAnswer = a - b;

        // Show question
        questionText.text = a + " - " + b + "?";
    }

    public void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                score += 10;
                ResetMathGame();
            }
            else
            {
                playerHearts--;
                UpdateUI();

                if (playerHearts <= 0)
                {
                    GameOver();
                }
            }
        }
        // Reset question input field after submitting
        answerInput.text = "";
    }

    public void PlayerHit()
    {
        // Player loses heart when hit by enemy
        playerHearts--;
        UpdateUI();

        if (playerHearts <= 0)
        {
            GameOver();
        }
    }

    void ResetMathGame()
    {
        answerInput.text = "";
        mathPanel.SetActive(false);
        isMathActive = false;
        UpdateUI();
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
    }

    IEnumerator ScoreIncrement()
    {
        // Increase score while game is ongoing
        while (playerHearts > 0)
        {
            yield return new WaitForSeconds(1);
            score++;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        scoreText.text = score;

        for (int i = 0; i < heartSprites.Length; i++)
        {
            if (i < playerHearts)
                heartSprites[i].sprite = fullHeart;
            else
                heartSprites[i].sprite = emptyHeart;
        }
    }

}
