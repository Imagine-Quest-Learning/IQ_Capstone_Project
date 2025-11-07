using UnityEngine;
using System.Collections.Generic;

/*
    File: GameManager

    Description: Ensures only 1 GameManager exists in the game
                 Makes sure certain objects persist when changing scenes
                 Any extra copies of GameManager are cleaned up automatically
    
    Written By: Ethan Tang [2025]
*/
public class GameManager : MonoBehaviour
{
    // Create globally acessible reference to single GameManager Instance
    public static GameManager Instance;

    // Array to store objects that should not be destroyed when loading new scene
    //      Filled from Unity Inspector
    public GameObject[] persistentObjects;

    /* HashSet to store completedRooms by SceneName
        - HashSet = faster lookups
    */
    public HashSet<string> completedRooms = new HashSet<string>();

    private void Awake()
    {
        // If there's already a GameManager Instance destroy it 
        if (Instance != null)
        {
            CleanUpAndDestroy();
            return;
        }
        else
        {
            // If this is first GameManager
            Instance = this; //set itself as the Instance
            DontDestroyOnLoad(gameObject); //Ensure this GameManager is NOT destroyed when a new scene loads
            MarkPersistentObjects(); //Apply persistance protection to all elements in array
        }
    }

    /*
        MarkPersistenObjects()
        - Loop through all persistent objects
        - For each non-null oject, make sure it survives scene change
    */
    private void MarkPersistentObjects()
    {
        if (persistentObjects == null) return;

        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null)
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    /*
        CleanUpAndDestory()
        - Runs if a duplicate GameManager appears
        - Destroys all its linked persistent objects and then itself 
    */
    private void CleanUpAndDestroy()
    {
        if (persistentObjects != null)
        {
            foreach (GameObject obj in persistentObjects)
            {
                Destroy(obj);
            }
        }
        Destroy(gameObject);
    }

    //PUBLIC METHODS FOR OTHER SCRIPTS TO USE BELOW
    public void MarkRoomComplete(string sceneName)
    {
        completedRooms.Add(sceneName);
    }
    
    public bool IsRoomComplete(string sceneName)
    {
        return completedRooms.Contains(sceneName);
    }
}

