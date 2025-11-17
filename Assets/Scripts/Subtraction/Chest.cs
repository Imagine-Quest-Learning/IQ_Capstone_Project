using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Popup UI Settings")]
    public GameObject chestPopupUI; //to assign popup panel from unity inspector
    public GameObject instructionsPopupUI;
    private bool isPlayerNearby = false;

    //Dictates whether or not the popup should work
    public bool popUpEnabled = true;

    void Start()
    {
        if (chestPopupUI != null)
        {
            chestPopupUI.SetActive(false); //keep popup hidden if not null 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered for PopUp!");
        if (popUpEnabled && collision.gameObject.tag == "Player")
        {
            isPlayerNearby = true;
            if (chestPopupUI != null)
            {
                chestPopupUI.SetActive(true);   
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNearby = false;
            if (chestPopupUI != null)
            {
                chestPopupUI.SetActive(false);
            }
        }
    }

    public void OnOpenButtonClicked()
    {
        CloseChestPopup();
        OpenInstructionsPopup();
    }

    public void OpenInstructionsPopup()
    {
        if(instructionsPopupUI != null)
        {
            instructionsPopupUI.SetActive(true);
        }
    }
    
    //method assigned to OpenMe Button
    public void CloseChestPopup()
    {
        Debug.Log("ClosePopup button pressed!");
        if (chestPopupUI != null)
        {
            chestPopupUI.SetActive(false);
        }
    }

    public void DisablePopup()
    {
        popUpEnabled = false;
    }
    
    public void EnablePopup()
    {
        popUpEnabled = true;
    }
}
