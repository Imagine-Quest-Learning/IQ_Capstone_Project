using UnityEngine;

public class Player : MonoBehaviour
{

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.position += movement * Time.deltaTime;
    }

}
