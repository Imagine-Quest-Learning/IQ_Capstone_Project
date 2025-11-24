using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

/* Written by Olivia Jia
   -> Testing timer controller monobehaviour script
*/
public class DivisionTimerControllerPlayModeTest
{
    private GameObject timerObj;
    private TimerController timerController;
    private Slider slider;

    //init
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        timerObj = new GameObject("TimerTestObject");
        timerController = timerObj.AddComponent<TimerController>();

        // slider
        var sliderObj = new GameObject("Slider");
        slider = sliderObj.AddComponent<Slider>();
        timerController.slider = slider;

        // set max time
        timerController.SetMaxTime(10f);

        yield return null; // let Start() run
    }

    //clean-up, not intefered with other test
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(timerObj);
        Object.Destroy(slider.gameObject);
        yield return null;
    }

    //test init timer visualy+logically
    [UnityTest]
    public IEnumerator Timer_Initializes_Correctly()
    {
        yield return null; // let Update()

        float tolerance = 0.5f; // small tolerance to account for time always running
        Assert.That(timerController.timeRemaining, Is.EqualTo(timerController.timeMax).Within(tolerance),"timeRemaining should be approximately equal to timeMax at start");
        Assert.That(timerController.slider.value, Is.EqualTo(timerController.timeMax).Within(tolerance),"Slider value should be approximately equal to timeMax at start");
    }

    //test decrease timer visualy+logically
    [UnityTest]
    public IEnumerator Timer_CountsDown_OverTime()
    {
        float startTime = timerController.timeRemaining;

        // simulate 2 second passing
        yield return new WaitForSeconds(2f);

        Assert.Less(timerController.timeRemaining, startTime, "timeRemaining should decrease over time");
        Assert.Less(timerController.slider.value, startTime, "Slider value should decrease along with timer");
    }

    //test reset timer visualy+logically
    [UnityTest]
    public IEnumerator Timer_ResetTimer_Works()
    {
        // simulate countdown
        timerController.timeRemaining = 5f;
        timerController.slider.value = 5f;

        timerController.ResetTimer();
        yield return null; // let Update()

        float tolerance = 0.5f; // small tolerance to account for time always running
        Assert.That(timerController.timeRemaining, Is.EqualTo(timerController.timeMax).Within(tolerance),"timeRemaining should be approximately equal to timeMax at start");
        Assert.That(timerController.slider.value, Is.EqualTo(timerController.timeMax).Within(tolerance),"Slider value should be approximately equal to timeMax at start");
    }

    //test set max timer visualy+logically
    [UnityTest]
    public IEnumerator Timer_SetMaxTime_Works()
    {
        timerController.SetMaxTime(20f);
        yield return null; // let Update()

        float tolerance = 0.5f; // small tolerance to account for time always running

        Assert.That(timerController.timeMax, Is.EqualTo(20f).Within(tolerance),"SetMaxTime should update timeMax");
        Assert.That(timerController.timeRemaining, Is.EqualTo(20f).Within(tolerance),"SetMaxTime should update timeRemaining");
        Assert.That(timerController.slider.value, Is.EqualTo(20f).Within(tolerance),"SetMaxTime should update slider value");
    }

}
