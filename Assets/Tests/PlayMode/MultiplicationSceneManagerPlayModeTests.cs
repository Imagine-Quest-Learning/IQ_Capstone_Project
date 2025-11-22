using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using UnityEngine.UI;

public class MultiplicationSceneManagerPlayModeTests
{
    private GameObject managerObject;
    private MultiplicationSceneManager sceneManager;

    private GameObject mathPanel;
    private GameObject instructionsPanel;
    private GameObject winPanel;
    private GameObject losePanel;

    private Text questionText;
    private InputField answerInput;
    private Text scoreText;

    private Image[] heartSprites;
    private Sprite fullHeart;
    private Sprite emptyHeart;

    private GameObject wandPrefab;

    [SetUp]
    public void Setup()
    {
        //Create a sceneManager and attach MultiplicationSceneManager
        managerObject = new GameObject("MultiplicationSceneManager");
        sceneManager = managerObject.AddComponent<MultiplicationSceneManager>();

        mathPanel = new GameObject("MathPanel");
        instructionsPanel = new GameObject("InstructionsPanel");
        winPanel = new GameObject("WinPanel");
        losePanel = new GameObject("LosePanel");
        questionText = new GameObject("QuestionText").AddComponent<Text>();
        answerInput = new GameObject("AnswerInput").AddComponent<InputField>();
        scoreText = new GameObject("ScoreText").AddComponent<Text>();

        sceneManager.mathPanel = mathPanel;
        sceneManager.instructionsPanel = instructionsPanel;
        sceneManager.winPanel = winPanel;
        sceneManager.losePanel = losePanel;

        sceneManager.questionText = questionText;
        sceneManager.answerInput = answerInput;
        sceneManager.scoreText = scoreText;

        // Heart initialization with fixed textures
        heartSprites = new Image[3];
        for (int i = 0; i < 3; i++)
        {
            var imgObj = new GameObject("Heart" + i).AddComponent<Image>();
            heartSprites[i] = imgObj;
        }

        sceneManager.heartSprites = heartSprites;

        Texture2D tex1 = new Texture2D(10, 10);
        Texture2D tex2 = new Texture2D(10, 10);

        fullHeart = Sprite.Create(tex1, new Rect(0, 0, 10, 10), Vector2.zero);
        emptyHeart = Sprite.Create(tex2, new Rect(0, 0, 10, 10), Vector2.zero);

        sceneManager.fullHeart = fullHeart;
        sceneManager.emptyHeart = emptyHeart;

        // Wand prefab
        wandPrefab = new GameObject("WandPrefab");
        sceneManager.wandPrefab = wandPrefab;

        managerObject.SetActive(true);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(managerObject);
        Object.DestroyImmediate(wandPrefab);

        //Reset static Instance to prevent test contamination
        GameManager.Instance = null;
    }

    [UnityTest]
    public IEnumerator StartGame_InitializesGameCorrectly()
    {
        yield return null;

        sceneManager.StartGame();
        yield return null;

        Assert.IsFalse(instructionsPanel.activeSelf, "Instructions should not be active after game starts.");
        Assert.IsTrue(sceneManager.isGameActive, "Game should be marked as active.");
        Assert.AreEqual(0, sceneManager.scoreText.text.StartsWith("0") ? 0 : -1, "Score should reset to 0/10.");
        Assert.AreEqual(3, sceneManager.playerHearts, "Player should start with 3 hearts.");
    }

    [UnityTest]
    public IEnumerator ShowMathQuestion_EnablesMathPanelAndGeneratesQuestion()
    {
        yield return null;
        sceneManager.StartGame();
        yield return null;

        sceneManager.SendMessage("ShowMathQuestion");
        yield return null;

        Assert.IsTrue(mathPanel.activeSelf, "Math panel should become visible when question appears.");
        Assert.IsTrue(sceneManager.isMathActive, "isMathActive should now be true.");
        Assert.IsFalse(string.IsNullOrEmpty(questionText.text), "Question text should not be empty.");
    }

    [UnityTest]
    public IEnumerator CheckAnswer_CorrectAnswer_IncreasesScoreAndClosesPanel()
    {
        yield return null;
        sceneManager.StartGame();
        yield return null;

        sceneManager.SendMessage("ShowMathQuestion");
        yield return null;

        // Force known answer
        sceneManager.SendMessage("GenerateMathProblem");
        int answer = int.Parse(sceneManager.questionText.text.Split('x')[0].Trim()) *
                     int.Parse(sceneManager.questionText.text.Split('x')[1].Replace("?", "").Trim());

        answerInput.text = answer.ToString();

        sceneManager.CheckAnswer();
        yield return null;

        Assert.AreEqual("1 / 10", sceneManager.scoreText.text, "Score should increase by 1.");
        Assert.IsFalse(mathPanel.activeSelf, "Math panel should hide after correct answer.");
        Assert.IsFalse(sceneManager.isMathActive);
    }

    [UnityTest]
    public IEnumerator CheckAnswer_WrongAnswer_DecreasesHearts()
    {
        yield return null;
        sceneManager.StartGame();
        yield return null;

        sceneManager.SendMessage("ShowMathQuestion");
        yield return null;

        answerInput.text = "-1"; // Always wrong
        sceneManager.CheckAnswer();
        yield return null;

        Assert.AreEqual(2, sceneManager.playerHearts, "Player should lose one heart.");
        Assert.AreEqual(emptyHeart, heartSprites[2].sprite, "Last heart should display empty sprite.");
    }

    [UnityTest]
    public IEnumerator PlayerHit_DecreasesHeartCount()
    {
        yield return null;
        sceneManager.StartGame();
        yield return null;

        sceneManager.PlayerHit();
        yield return null;

        Assert.AreEqual(2, sceneManager.playerHearts, "PlayerHit() should remove 1 heart.");
    }

    [UnityTest]
    public IEnumerator ClearAllEnemies_DestroysEnemiesInScene()
    {
        yield return null;

        var enemy1 = new GameObject("Enemy1");
        enemy1.tag = "Enemy";

        var enemy2 = new GameObject("Enemy2");
        enemy2.tag = "Enemy";

        Assert.AreEqual(2, GameObject.FindGameObjectsWithTag("Enemy").Length);

        sceneManager.ClearAllEnemies();
        yield return null;

        Assert.AreEqual(0, GameObject.FindGameObjectsWithTag("Enemy").Length, "All enemies should be destroyed.");
    }
}
