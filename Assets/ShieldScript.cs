using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    public LogicManagerScript logicScript;
    public AstroidSpawnerScript astroidSpawnerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
        astroidSpawnerScript = GameObject.FindGameObjectWithTag("AstroidSpawner").GetComponent<AstroidSpawnerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Astroid") 
        {
            //Prevent multiple triggers of collider
            Collider2D astroidCollider = collision.GetComponent<Collider2D>();
            if (astroidCollider != null)
            {
                astroidCollider.enabled = false;
            }
            //Get rigid body of astroid
            Rigidbody2D astroidRb = collision.gameObject.GetComponent<Rigidbody2D>();

            if(astroidRb != null)
            {
                astroidRb.linearVelocityX = 10;
            }

            Debug.Log("Shield Strength MINUS 1.");
            logicScript.decreaseShieldStrength();

            Debug.Log("Destroy Astroid Hitting Shield.");
            Destroy(collision.gameObject, 0.5f); //Allow time for bounce before deleting astroid

            logicScript.EndMultiplicationQuestion();
        }
    }
}
