using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class LogicManagerScript : MonoBehaviour
{
    //References to Scripts/Objects
    public AstroidSpawnerScript astroidSpawnerScript;

    //Shield Strength Variables
    public int shieldStrength = 5;
    public Text shieldStrengthText;

    //Multiplication Question Variables
    public GameObject multiplicationUI;
    public Text questionText;
    public InputField answerInput;
    private GameObject currentAstroid;
    private int correctAns;
    private bool spawnLock = false;

    void Start()
    {
        spawnLock = false;
        astroidSpawnerScript = GameObject.FindGameObjectWithTag("AstroidSpawner").GetComponent<AstroidSpawnerScript>();
    }

    [ContextMenu("Decrease Shield Strength")]
    public void decreaseShieldStrength(){
        shieldStrength = shieldStrength - 1;
        shieldStrengthText.text = shieldStrength.ToString();

        //will need to add function for gameover when shieldstrength = 0;
    }


    //FUNCTIONS FOR MULTIPLICATION QUESTIONS FOLLOW
    public void StartMultiplicationQuestion(GameObject astroid)
    {
        currentAstroid = astroid;
        multiplicationUI.SetActive(true);
        GenerateMultiplicationQuestion();
    }

    private void GenerateMultiplicationQuestion()
    {
        //Generate a random multiplication question
        int num1 = Random.Range(1, 10);
        int num2 = Random.Range(1, 10);
        correctAns = num1 * num2;

        Debug.Log(num1);
        Debug.Log(num2);
        Debug.Log(correctAns);

        //Display the question
        questionText.text = $"{num1} x {num2} = ";
    }

    public void SubmitAnswer()
    {
        if (spawnLock) return;

        int playerAnswer;

        if(int.TryParse(answerInput.text, out playerAnswer))
        {
            if(playerAnswer == correctAns)
            {
                //increase speed by 1f
                astroidSpawnerScript.increaseAstroidSpeed();

                DestroyAstroid();
                Debug.Log("Correct Answer, Speeding Up!");
                EndMultiplicationQuestion();
            }
            else
            {  
                //decrease speed by 1f
                astroidSpawnerScript.decreaseAstroidSpeed();
                Debug.Log("Incorrect Answer, Slowing Down.");
            }
        }
    }

    public void EndMultiplicationQuestion()
    {
        if (spawnLock) return;
        spawnLock = true;

        Debug.Log("EndMultiplicationSpawn");
        multiplicationUI.SetActive(false);
        answerInput.text = "";
        answerInput.Select();
        astroidSpawnerScript.spawnAstroid();

        //Reset spawnlock with delay
        StartCoroutine(ResetSpawnLock());
    }

    private IEnumerator ResetSpawnLock()
    {
        yield return new WaitForSeconds(0.1f);
        spawnLock = false;
    }

    private void DestroyAstroid()
    {
        if(currentAstroid != null)
        {
            Destroy(currentAstroid);
            Debug.Log("Astroid Destroyed");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SubmitAnswer();
        }
    }
}
