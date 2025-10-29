using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls visuals and logical of player health
public class PlayerHealth : MonoBehaviour
{
    public GameObject[] hearts;
    public int life;

 
    // Update is called once per frame
    void Update()
    {
        //removes one of players lives visually
        if (life<1){
            Destroy(hearts[0].gameObject);
            //set game over
        }else if (life<2){
            Destroy(hearts[1].gameObject);
        }else if (life<3){
            Destroy(hearts[2].gameObject);
        }
    }

    public void TakeDamage(int damage)
	{
		life -= damage;
	}
}
