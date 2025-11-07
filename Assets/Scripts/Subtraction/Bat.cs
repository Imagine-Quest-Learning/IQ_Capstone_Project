using UnityEngine;

public class Bat : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        //Move bat to the right
        transform.position = transform.position + (Vector3.right * moveSpeed) * Time.deltaTime;
    }

    public void setMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}
