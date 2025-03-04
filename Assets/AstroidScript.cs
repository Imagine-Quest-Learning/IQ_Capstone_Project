using UnityEngine;

public class AstroidScript : MonoBehaviour
{
    public float moveSpeed = 5f;
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

    }

    public void setSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
