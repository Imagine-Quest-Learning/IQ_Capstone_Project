using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DivisionWinScreenControllerPlayModeTests
{
    private GameObject testObject;
    private WinScreenController controller;

    //initialisation
    [SetUp]
    public void SetUp()
    {
        testObject = new GameObject("WinScreenTestObject");
        controller = testObject.AddComponent<WinScreenController>();
        testObject.SetActive(false);
    }

    //clean-up, not intefered with other test
    [TearDown]
    public void TearDown()
    {
        Object.Destroy(testObject);
    }

    //testing activation
    [UnityTest]
    public IEnumerator ScreenObjectActive()
    {
        yield return null;
        controller.ActivateScreen();
        yield return null;
        Assert.IsTrue(testObject.activeSelf, "Win Screen did not activate.");
    }
}
