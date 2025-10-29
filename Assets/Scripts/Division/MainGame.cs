using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Main logic of the game and the question generator/verifyer
public class DivisionAttack : MonoBehaviour
{
    [SerializeField] InputField answer;
    [SerializeField] Text question;
    public Golem golem;
    public PlayerHealth playerHealth;
    // public LoseScreenController lose;
    public WinScreenController win;
    public TimerController timer;
    public float timeMax = 25;
    float baseAttack = 20;
    int attack;

    int x;//second term in question
    int y;//valid answer
    int z; //first term in question
    bool canAnswer = true; // lock/unlock input field for answers

    // Start is called before the first frame update
    void Start()
    {
       answer.Select();
       timer.SetMaxTime(timeMax);
       NextQuestion();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.timeRemaining <= 0 && canAnswer)
        {
            // for when timer runs out
            canAnswer = false;
            playerHealth.TakeDamage(1);

            //Check if player lost and display game over screen if they did
            if (playerHealth.life<=0){
                GameOver();
                return;
            }else{

                //moves on to next question
                NextQuestion();
                return;
            }           
        }
        if (Input.GetKeyDown(KeyCode.Return) && canAnswer)
		{
		    CheckAnswer(); 
		}   
    }

    void CheckAnswer(){
        string playerAnswer = answer.text;//captures player's answer

        if(playerAnswer == y.ToString()){

            //golem lose health and player is given next question
            attack = (int)Mathf.Floor(baseAttack * (timer.timeRemaining/timer.timeMax));
            golem.TakeDamage(attack);

            //Check if player won and display game won screen if they did
            if (golem.currentHealth<=0){
                GameWin();
                return;
            }else{

                //moves on to next question
                NextQuestion();
                return;
            }   
        }else{

            //player lose life for wrong answer
            playerHealth.TakeDamage(1);

            //Check if player lost and display game over screen if they did
            if (playerHealth.life<=0){
                GameOver();
                return;
            }
            
            question.color = Color.red;    
            answer.text = "";
            answer.Select();
            answer.ActivateInputField();  
        }
    }

    void NextQuestion(){

        //new generated question
        x = Random.Range(1,15);
        y = Random.Range(0,15);
        z = x*y;

        //update + reset text and question box
        answer.text = "";
        answer.Select();
        answer.ActivateInputField();
        question.text = z.ToString() + "/" + x.ToString() + " = ?";
        question.color = Color.white;

        //resetting timer
        timer.ResetTimer();
        canAnswer = true;
        
    }

    void GameOver(){
        canAnswer = false;
        SceneManager.LoadScene(0);
    }

    void GameWin(){
        canAnswer = false;
        win.ActivateScreen();
    }
}