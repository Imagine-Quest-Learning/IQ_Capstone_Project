using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonLauncher : MonoBehaviour
{
    public Rigidbody2D rb;
    public int PowerUP;
    public int PowerRIGHT;
    public float WaitTime = 2f;
    private bool Shot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Shot = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Shot == true)
        {
            rb.AddForce(Vector2.up * PowerUP);
            rb.AddForce(Vector2.right * PowerRIGHT);
            Shot = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            StartCoroutine(WaitForIt(WaitTime));
        }

        if (col.gameObject.tag == "Launcher")
        {
            Shot = true;
        }
    }

    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        transform.position = new Vector2(0, -2);
        rb.linearVelocity = Vector3.zero;
        Shot = false;
    }
}
