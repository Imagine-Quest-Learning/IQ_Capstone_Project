using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/*
    File: GameManagerTests

    Description: Holds PLAYMODE unit tests for the GameManager.cs
    
    Written By: Jaiden Clee [2025]
*/


public class GameManagerTests
{
    private GameObject gameManagerObject;
    private GameManager gameManager;

    [SetUp]
    public void Setup()
    {
        //Create a GameObject and attach GameManager
        gameManagerObject = new GameObject("GameManager");

        //Create persistent objects
        GameObject persistentObj1 = new GameObject("PersistentObject1");
        GameObject persistentObj2 = new GameObject("PersistentObject2");

        //Add component After setting up persistent objects to avoid nullexceptionerror
        gameManager = gameManagerObject.AddComponent<GameManager>();
        gameManager.persistentObjects = new GameObject[] { persistentObj1, persistentObj2 };
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(gameManagerObject);
        foreach (var obj in gameManager.persistentObjects)
        {
            if (obj != null)
                Object.DestroyImmediate(obj);
        }

        //Reset static Instance to prevent test contamination
        GameManager.Instance = null;
    }

    [UnityTest]
    public IEnumerator SingletonInstanceAssignedOnAwake()
    {
        gameManagerObject.SetActive(true);
        yield return null;

        Assert.AreEqual(gameManager, GameManager.Instance, "GameManager.Instance should point to this instance.");
    }

    [UnityTest]
    public IEnumerator PersistenObjectsAreNotDestroyedOnLoad()
    {
        gameManagerObject.SetActive(true);
        yield return null; //Awake should run automatically here

        foreach (var obj in gameManager.persistentObjects)
        {
            Assert.IsNotNull(obj, "Persistent object should still exist after Awake");
        }
    }

    [UnityTest]
    public IEnumerator DuplicateManagerIsDestroyed()
    {
        //First instance
        gameManagerObject.SetActive(true);
        yield return null;

        //Create duplicate
        var duplicateObject = new GameObject("DuplicateManager");
        var duplicate = duplicateObject.AddComponent<GameManager>();
        duplicate.persistentObjects = new GameObject[] { new GameObject("DupObj") };

        //Simulate awake call for duplicate object
        duplicateObject.SetActive(true);
        yield return null;

        //Verify deplicate destroyed itself
        Assert.IsTrue(duplicate == null || duplicate.Equals(null), "Duplicate GameManager should have destroyed itself");

        Object.DestroyImmediate(duplicateObject);
    }
}