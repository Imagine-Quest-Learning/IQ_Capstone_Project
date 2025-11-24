using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Written by Olivia Jia
    -> controls visuals and logical of player health
*/
public class PlayerHealth : MonoBehaviour
{
    //initialisation + reference game objects
    public GameObject[] hearts;
    public int life;

   //update following every frame
    void Update()
    {
        //removes one of players lives visually
        if (life<1){
            Destroy(hearts[0].gameObject);

        }else if (life<2){
            Destroy(hearts[1].gameObject);
        }else if (life<3){
            Destroy(hearts[2].gameObject);
        }
    }

    //logical removal of hearts
    public void TakeDamage(int damage)
	{
		life -= damage;
	}
}
