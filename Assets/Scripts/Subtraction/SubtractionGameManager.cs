using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SubtractionGameManager : MonoBehaviour
{
    public static SubtractionGameManager Instance { get; private set; }

    //VARS TO SET VIA INSPECTOR
    [Header("Bat Game Settings")]
    public BatSpawner batSpawner;
    public BarrelManager barrelManager;

    [Header("UI References")]
    public GameObject chestPopup;
    public GameObject winLosePopup;
    public Chest chest;
    public Text questionTextUI;
    public InputField answerInputField;
    public Text questionsLeftTextUI;
    public Text feedbackTextUI;
    public Text winLoseTextUI;

    //PRIVATE VARS
    private int currentCorrectAnswer;
    private List<GameObject> batsList = new List<GameObject>(); //to keep track of bat spawning order
    private int correctAnswers; //set when game is started (in case player loses and game resets)
    private int totalToWin; //set when game is started (in case player loses and game resets)
    private bool isWinLosePanelActive = false;
    private float winLosePopupShownTime;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if(isWinLosePanelActive && Input.GetKeyDown(KeyCode.Return) && Time.time - winLosePopupShownTime > 1f)
        {
            winLosePopup.SetActive(false);
            isWinLosePanelActive = false;
        }
    }

    public void StartSubtractionGame()
    {
        //set game counters
        totalToWin = 15;
        correctAnswers = 0;

        //disable popup
        if (chestPopup != null)
        {
            chestPopup.SetActive(false);
            chest.DisablePopup();
        }

        //enable subtraction game ui 
        if (questionTextUI != null)
        {
            questionTextUI.gameObject.SetActive(true);
        }
        if (answerInputField != null)
        {
            answerInputField.gameObject.SetActive(true);
        }
        if (questionsLeftTextUI != null)
        {
            questionsLeftTextUI.gameObject.SetActive(true);
        }

        //focus cursor on input field
        FocusInputField();

        //move player to right side of screen and disable movement
        if (Player.Instance != null)
        {
            Vector3 newPosition = new Vector3(1.154f, -0.005f, 0f);
            Player.Instance.SetPlayerPosition(newPosition);
            Player.Instance.SetCanMove(false);
        }

        //initialize barrels
        barrelManager.Initialize();

        //start bat spawner
        if (batSpawner != null)
        {
            batSpawner.StartSpawning();
        }

        //generate first question
        GenerateNewQuestion();

        //Listen to input field submit
        if (answerInputField != null)
        {
            answerInputField.onEndEdit.RemoveAllListeners(); //needed if player loses and game restarts
            answerInputField.onEndEdit.AddListener(CheckPlayerAnswer);
        }
    }

    private void FocusInputField()
    {
        if (answerInputField != null)
        {
            answerInputField.ActivateInputField();
        }
    }

    private void UpdateQuestionsLeftUI()
    {
        if (questionsLeftTextUI != null)
        {
            int remaining = totalToWin - correctAnswers;
            questionsLeftTextUI.text = $"{remaining} Left!";
        }
    }
    
    private IEnumerator ShowCorrectAnswer(string correctAnswer)
    {
        if(feedbackTextUI != null)
        {
            feedbackTextUI.text = $"{correctAnswer}";
            feedbackTextUI.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            feedbackTextUI.gameObject.SetActive(false);
        }
    }

    private void GenerateNewQuestion()
    {
        var (question, answer) = SubtractionUtils.GenerateQuestion();
        currentCorrectAnswer = answer;

        //set question UI to newly generated question
        if (questionTextUI != null)
        {
            questionTextUI.text = question;
        }
    }

    public void OnCorrectAnswer()
    {
        correctAnswers++;
        UpdateQuestionsLeftUI();

        batSpawner.DecreaseSpawnRate(); //spawn bats faster when correct answer is given!
        batSpawner.SpawnSingleBat();

        if(correctAnswers >= totalToWin && barrelManager.GetRemainingBarrels()>0)
        {
            OnPlayerWon();
        }
    }

    //Called when player submits an answer
    private void CheckPlayerAnswer(string playerInput)
    {
        if (SubtractionUtils.IsAnswerCorrect(playerInput, currentCorrectAnswer))
        {
            Debug.Log("Correct Answer!");
            OnCorrectAnswer();
            DestroyNearestBat();
        }
        else
        {
            StartCoroutine(ShowCorrectAnswer(currentCorrectAnswer.ToString()));
            Debug.Log("Incorrect Answer!");

        }
        //clear input 
        answerInputField.text = "";

        //focus input field
        FocusInputField();

        //generate next question
        GenerateNewQuestion();
    }

    //Add a new bat to the queue
    public void RegisterBat(GameObject newBat)
    {
        batsList.Add(newBat);
    }

    private void DestroyNearestBat()
    {
        //destroy the oldest bat in the list
        if (batsList.Count > 0)
        {
            GameObject batToDestroy = batsList[0];
            batsList.RemoveAt(0);
            if(batToDestroy != null)
            {
                Destroy(batToDestroy);
            }
        }
    }

    public void RemoveBatFromList(GameObject bat)
    {
        if(batsList.Contains(bat))
        {
            batsList.Remove(bat);
        }
    }

    public void OnPlayerWon()
    {
        Debug.Log("Player WON!");
        EndGame();
        ShowWinLosePopup(true);

        //mark room completed
        GameManager.Instance.MarkRoomComplete("Subtraction");
    }

    public void OnPlayerLost()
    {
        Debug.Log("Player lost :(");

        //only re-enable popup if player lost
        if (chestPopup != null)
        {
            chestPopup.SetActive(false);
            chest.EnablePopup();
        }
        EndGame();
        ShowWinLosePopup(false);
    }

    private void ShowWinLosePopup(bool didWin)
    {
        if (winLosePopup != null && winLoseTextUI != null)
        {
            winLosePopup.SetActive(true);
            winLoseTextUI.text = didWin ? "Congrats, you've defeated the bats!\n\nLet's keep moving..." : "The bats have defeated you...\n\nLet's try again!";
            isWinLosePanelActive = true;
            winLosePopupShownTime = Time.time;
        }
    }
    
    public void EndGame()
    {
        //allow player to move
        Player.Instance.SetCanMove(true);

        //disable game components
        if (questionTextUI != null) questionTextUI.gameObject.SetActive(false);
        if (answerInputField != null) answerInputField.gameObject.SetActive(false);
        if (questionsLeftTextUI != null) questionsLeftTextUI.gameObject.SetActive(false);
        if (feedbackTextUI != null) feedbackTextUI.gameObject.SetActive(false);

        //reset some game components
        answerInputField.text = "";
        questionsLeftTextUI.text = "";
        feedbackTextUI.text = "";

        //turn off bat spawner
        if (batSpawner != null) batSpawner.StopSpawning();

        //rehide barrels
        if (barrelManager != null) barrelManager.HideAllBarrels();
    }
}
