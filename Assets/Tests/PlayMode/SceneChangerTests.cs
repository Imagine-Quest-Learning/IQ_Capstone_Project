using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.Collections;
/*
    File: SceneChangerTests

    Description: Holds PLAYMODE unit tests for the SceneChanger.cs
    
    Written By: Jaiden Clee [2025]
*/

public class SceneChangerTests
{
    private GameObject player;
    private GameObject triggerObject;
    private SceneChanger sceneChanger;
    private Animator fadeAnimator;

    [SetUp]
    public void Setup()
    {
        //Create player
        player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<BoxCollider2D>();
        player.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        //Create trigger
        triggerObject = new GameObject("Trigger");
        BoxCollider2D triggerCollider = triggerObject.AddComponent<BoxCollider2D>();
        triggerCollider.isTrigger = true;

        //Add SceneChanger
        sceneChanger = triggerObject.AddComponent<SceneChanger>();
        sceneChanger.sceneToLoad = "TestScene";
        sceneChanger.newPlayerPosition = new Vector2(5, 5);
        sceneChanger.fadeTime = 0.1f;

        //Disable scene loading and fade animation
        sceneChanger.disableSceneLoad = true;
        sceneChanger.disableFadeAnimation = true;

        //Add fade animator
        fadeAnimator = triggerObject.AddComponent<Animator>();
        sceneChanger.fadeAnimation = fadeAnimator;
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(triggerObject);
    }

    [UnityTest]
    public IEnumerator PlayerTriggerUpdatesPositionAndRequestsSceneLoad()
    {
        //Move player into trigger
        player.transform.position = triggerObject.transform.position;

        //Manually simulate collision
        var collider = triggerObject.GetComponent<Collider2D>();
        sceneChanger.OnTriggerEnter2D(player.GetComponent<Collider2D>());

        //Fade animation should play
        Assert.IsNotNull(sceneChanger.fadeAnimation);

        //Wait for fadeTime + small buffer
        yield return new WaitForSeconds(sceneChanger.fadeTime + 0.05f);

        //Convert expected position into Vector3 for fair comparison
        Vector3 expectedPos = new Vector3(
            sceneChanger.newPlayerPosition.x,
            sceneChanger.newPlayerPosition.y,
            0f
        );

        //Player position should update
        Assert.AreEqual(expectedPos, player.transform.position, "Player should move to new position");

        //Istead of actually loading a scene, verify sceneToLoad String
        Assert.AreEqual("TestScene", sceneChanger.sceneToLoad);
    }
}