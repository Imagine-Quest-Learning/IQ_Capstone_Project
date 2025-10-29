using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// controls visuals and logical of timer bar

public class TimerController : MonoBehaviour
{

    public Image timeBar;
    public float timeRemaining;
    public float timeMax;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
       timeRemaining = timeMax; 
       //tryAgainButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0){
            timeRemaining -= Time.deltaTime;
            slider.value = timeMax * (timeRemaining/timeMax);
        }  
    }

    //setting maximum time
    public void SetMaxTime(float time){
		slider.maxValue = time;
		slider.value = time;
        timeMax = time;
        timeRemaining = timeMax;
	}

    //restarting timer -- for every question
    public void ResetTimer()
    {
        timeRemaining = timeMax;
        slider.value = timeMax;
    }
}
