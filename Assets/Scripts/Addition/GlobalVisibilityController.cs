using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    File: GlobalVisibilityController

    Description: Turns the global Player and global Camera on or off
                 depending on which scene is loaded.

    Written By: Jiayi(Jemma) [2025]
*/

public class GlobalVisibilityController : MonoBehaviour
{
    public GameObject globalPlayer;
    public GameObject globalCamera;

    // Names of scenes where the global Player/Camera should be hidden
    public string[] hideInScenes = { };

    private void OnEnable()
    {
        // Listen for scene load events
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Stop listening when this script is disabled
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if current scene is in the hide list
        bool shouldHide = hideInScenes.Contains(scene.name);

        // Show or hide the global Player
        if (globalPlayer != null)
            globalPlayer.SetActive(!shouldHide);

        // Show or hide the global Camera
        if (globalCamera != null)
            globalCamera.SetActive(!shouldHide);
    }
}
