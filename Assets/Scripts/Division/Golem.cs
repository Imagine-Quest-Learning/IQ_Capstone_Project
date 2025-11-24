using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Written by Olivia Jia
    -> controls visuals and logical of golem enemy health
*/
public class Golem : MonoBehaviour
{
    //initialisation + reference game objects
    public int baseHealth = 150;
    public int currentHealth;
    public HealthBar healthBar;

    //set before the first frame update
    void Start()
    {
        currentHealth = baseHealth;
		healthBar.SetMaxHealth(baseHealth);
    }

    //logical+visual removal of health
    public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}
