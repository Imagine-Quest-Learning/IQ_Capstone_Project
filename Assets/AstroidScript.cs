using UnityEngine;

public class AstroidScript : MonoBehaviour
{
    public float moveSpeed = 3;
    public float deadZone = -15;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Time.deltaTime ensures the speed does not depend on system's frame rate
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        //once the astroid has moved "off screen"
        if (transform.position.x < deadZone)
        {
            Debug.Log("Astroid deleted.");
            Destroy(gameObject); //destroy the gameObject that holds this script
        }
    }
}
