using UnityEngine;
using UnityEngine.SceneManagement;

// Manages background music persistence and destruction based on scene loading
public class Music1Manager : MonoBehaviour
{
    private static Music1Manager instance;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Called when the object becomes enabled and active
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Called when the behaviour becomes disabled
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called each time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the loaded scene is not "startpage" or "instructionpage", destroy this music manager
        if (scene.name != "startpage" && scene.name != "instructionpage")
        {
            Destroy(gameObject);
        }
    }
}
