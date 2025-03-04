using UnityEngine;

public class QuestionTriggerScript : MonoBehaviour
{
    public LogicManagerScript logicScript;
    public GameObject currentAstroid;

    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astroid")
        {
            //get current astroid
            currentAstroid = collision.gameObject;

            //start multiplication questions
            logicScript.StartMultiplicationQuestion(currentAstroid);
        }
    }
}
