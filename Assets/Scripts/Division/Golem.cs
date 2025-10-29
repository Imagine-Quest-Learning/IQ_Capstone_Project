using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls visuals and logical of golem enemy health
public class Golem : MonoBehaviour
{
    public int baseHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = baseHealth;
		healthBar.SetMaxHealth(baseHealth);
    }

    public void TakeDamage(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}
}
