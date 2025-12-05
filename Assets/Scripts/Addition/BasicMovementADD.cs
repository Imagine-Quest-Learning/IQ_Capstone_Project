using UnityEngine;

public class BasicMovementADD : MonoBehaviour
{
    public float speed = 1f;
    public bool canMove = true;

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    // Runs every frame
    void Update()
    {
        if (!canMove) return;
        Vector3 movement = new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            0f
        );

        // Move the object based on input
        transform.position += movement * Time.deltaTime;
    }
}
