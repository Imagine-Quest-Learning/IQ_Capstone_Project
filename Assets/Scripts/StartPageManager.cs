using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Manages the start page button interactions
public class StartPageManager : MonoBehaviour
{
    public Button startButton;

    // Assign button click listeners at the start
    void Start()
    {
        startButton.onClick.AddListener(LoadGamePage);
    }

    // Load the main game scene
    void LoadGamePage()
    {
        SceneManager.LoadScene("IntroductionPage");
    }
}
