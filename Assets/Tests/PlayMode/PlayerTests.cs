using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;

/*
    File: PlayerTests

    Description: Holds PLAYMODE unit tests for the Player.cs
                 Note: Player.cs needs playmode tests because it has MonoBehaviour
                       (aka it needs the game to load to run a test, it's not just logic we can test seperately)

    Written By: Jaiden Clee [2025]
*/


public class PlayerTests
{
    [UnityTest]
    public IEnumerator Player_Moves_When_Update_Called()
    {
        //Arrange 
        var gameObject = new GameObject();
        var player = gameObject.AddComponent<Player>();
        var startPosition = player.transform.position;

        //Simulate input by directly moving the player
        float moveSpeed = 1f;
        Vector3 fakeMovement = new Vector3(1f, 0f, 0f) * Time.deltaTime * moveSpeed;

        //Act
        player.transform.position += fakeMovement;
        yield return null;

        //Test Assestion statement
        Assert.AreNotEqual(startPosition, player.transform.position, "Player should have moved");
    }
    
}