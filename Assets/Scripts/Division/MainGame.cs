using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Written by Olivia Jia
    -> Main logic of the game:
        - Question generator/display/verifyer
        - Decrease player/golem health
        - Verify remaining time
*/
public class DivisionAttack : MonoBehaviour
{
    //init + reference objects
    [SerializeField] InputField answer;
    [SerializeField] Text question;
    public Golem golem;
    public PlayerHealth playerHealth;
    public WinScreenController win;
    public TimerController timer;
    public float timeMax = 10;
    float baseAttack = 20;
    int attack;

    int x;//second term in question
    int y;//valid answer
    int z; //first term in question
    bool canAnswer = true; // lock/unlock input field for answers

    //set before the first frame update
    void Start()
    {
       answer.Select();
       timer.SetMaxTime(timeMax);
       NextQuestion();
    }

    //update following every frame
    void Update()
    {
        if (timer.timeRemaining <= 0 && canAnswer)
        {
            // for when timer runs out
            canAnswer = false;
            playerHealth.TakeDamage(1);

            //Check if player lost and switch to main hall if they did
            if (playerHealth.life<=0){
                GameOver();
                return;
            }else{

                //moves on to next question
                NextQuestion();
                return;
            }           
        }
        //Capture/verify answers
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
            
            //change color text for incorrect answer
            question.color = Color.red;    
            answer.text = "";
            answer.Select();
            answer.ActivateInputField();  
        }
    }

    void NextQuestion(){

        //new generated question
        x = Random.Range(1,13);
        y = Random.Range(0,13);
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