using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// controls visuals and logical of health bar
public class HealthBar : MonoBehaviour
{
	public Slider slider; // controls amount of health visually
	public Gradient gradient; // gradient going from green (high health) to yellow (medium health) to red (low health)
	public Image fill; // display health amount


	public void SetHealth(int health){
		slider.value = health;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

	public void SetMaxHealth(int health){
		slider.maxValue = health;
		slider.value = health;
		fill.color = gradient.Evaluate(1f);
	}
 
}
