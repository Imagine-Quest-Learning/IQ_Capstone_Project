using UnityEngine;

/*
    Description: Makes the object slowly rotate, 
                 used for pieces of key on the game page.

*/
public class FloatAnimationGamePage : MonoBehaviour
{
    public float floatAmount = 0.2f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 15f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Make it bob up and down using a sine wave
        transform.position = startPos + new Vector3(
            0f,
            Mathf.Sin(Time.time * floatSpeed) * floatAmount,
            0f
        );

        // Make it slowly rotate around Z axis
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}
