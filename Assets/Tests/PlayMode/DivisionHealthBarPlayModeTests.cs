using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.Collections;

/* Written by Olivia Jia
    -> Testing health bar monobehavior script
*/
public class DivisionHealthBarPlayModeTests
{
    private GameObject hbObj;
    private HealthBar healthBar;
    private Slider slider;
    private Image fill;

    //init
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // create parent canvas for UI
        new GameObject("Canvas", typeof(Canvas));

        hbObj = new GameObject("HealthBarTestObject");
        slider = hbObj.AddComponent<Slider>();

        var fillObj = new GameObject("Fill");
        fill = fillObj.AddComponent<Image>();

        fillObj.transform.SetParent(hbObj.transform);
        healthBar = hbObj.AddComponent<HealthBar>();

        // references
        healthBar.slider = slider;
        healthBar.fill = fill;

        // gradient key -- percentage is where color is, not begins/ends (could be though)
        healthBar.gradient = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(new Color32(156, 7, 7, 255),   0.0f),//red, 0%
                new GradientColorKey(new Color32(255, 193, 7, 255), 0.30f),//yellow, 30%
                new GradientColorKey(new Color32(45, 184, 39, 255),   1.0f),//green, 100%
            }
        };

        yield return null; // let init
    }

    //clean-up, not intefered with other test
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(hbObj);
        yield return null;
    }

    // testing settting max health
    [UnityTest]
    public IEnumerator SetMaxHealth_SetsSliderCorrectly()
    {
        healthBar.SetMaxHealth(100);
        yield return null;

        Assert.AreEqual(100, slider.maxValue, "Max value is not matching input.");
        Assert.AreEqual(100, slider.value, "Value has not been set to max.");
        Assert.AreEqual(healthBar.gradient.Evaluate(1f),fill.color,"Fill color does not match gradient max value.");
    }

    // testing settting health
    [UnityTest]
    public IEnumerator SetHealth_UpdatesSliderAndColor()
    {
        //init max
        healthBar.SetMaxHealth(100);
        yield return null;// let update

        //set halfway
        healthBar.SetHealth(50);
        yield return null;// let update

        Assert.AreEqual(50, slider.value, "Slider value has not been updated.");

        Color expectedColor = healthBar.gradient.Evaluate(0.5f);
        Assert.AreEqual(expectedColor, fill.color, "Fill color does not match gradient key.");
    }
}
