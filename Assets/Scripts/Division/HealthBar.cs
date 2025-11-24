using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Written by Olivia Jia
    -> controls visuals and logical of health bar (for golem)
*/
public class HealthBar : MonoBehaviour
{
	//initialisation + reference game objects
	public Slider slider; // controls amount of health visually
	public Gradient gradient; // gradient going from green (high health) to yellow (medium health) to red (low health)
	public Image fill; // display health amount

	//sets health
	public void SetHealth(int health){
		slider.value = health;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

	//set max (for init)
	public void SetMaxHealth(int health){
		slider.maxValue = health;
		slider.value = health;
		fill.color = gradient.Evaluate(1f);
	}
 
}
