using UnityEngine;

public class SubtractionGameManager : MonoBehaviour
{
    [Header("Bat Game Settings")]
    public BatSpawner batSpawner;

    [Header("UI References")]
    public GameObject chestPopup;
    public Chest chest;
    
    public void StartSubtractionGame()
    {
        //disable popup
        if (chestPopup != null)
        {
            chestPopup.SetActive(false);
            chest.DisablePopup();
        }
        
        //start bat spawner
        if (batSpawner != null)
        {
            batSpawner.StartSpawning();
        }
        
        
    }
}
