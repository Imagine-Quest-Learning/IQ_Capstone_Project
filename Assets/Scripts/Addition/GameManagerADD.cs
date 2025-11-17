using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManageraADD : MonoBehaviour
{
    public static GameManageraADD Instance;
    public GameObject[] persistentObjects;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        MarkPersistentObjects();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void MarkPersistentObjects()
    {
        foreach (var obj in persistentObjects)
        {
            if (obj == null) continue;
            if (obj.GetComponentInChildren<Camera>(true) != null) continue;
            if (obj.GetComponentInChildren<AudioListener>(true) != null) continue;
            DontDestroyOnLoad(obj);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "gamepage") return;

        var main = Camera.main;
        var listeners = GetAll<AudioListener>(includeInactive: true);
        foreach (var al in listeners)
            if (main == null || al.gameObject != main.gameObject)
                Destroy(al);
    }

    static T[] GetAll<T>(bool includeInactive) where T : Object
    {
#if UNITY_6000_0_OR_NEWER || UNITY_2023_2_OR_NEWER
        return Object.FindObjectsByType<T>(
            includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude,
            FindObjectsSortMode.None);
#elif UNITY_2023_1_OR_NEWER
        return Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
        return Object.FindObjectsOfType<T>(includeInactive);
#endif
    }
}
