using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] hearts;
    public int life;

 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			TakeDamage(1);
		}

        if (life<1){
            Destroy(hearts[0].gameObject);
            //set game over
        }else if (life<2){
            Destroy(hearts[1].gameObject);
        }else if (life<3){
            Destroy(hearts[2].gameObject);
        }
    }

    void TakeDamage(int damage)
	{
		life -= damage;
	}
}
