using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Popup UI Settings")]
    public GameObject popupUI; //to assign popup panel from unity inspector
    private bool isPlayerNearby = false;

    //Dictates whether or not the popup should work
    public bool popUpEnabled = true;

    void Start()
    {   
        popupUI?.SetActive(false); //keep popup hidden if not null
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Entered for PopUp!");
        if (popUpEnabled && collision.gameObject.tag == "Player")
        {
            isPlayerNearby = true;
            if (popupUI != null)
            {
                popupUI.SetActive(true);   
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerNearby = false;
            if (popupUI != null)
            {
                popupUI.SetActive(false);   
            }
        }
    }

    //method assigned to OpenMe Button
    public void ClosePopup()
    {
        Debug.Log("ClosePopup button pressed!");
        if (popupUI != null)
        {
            popupUI.SetActive(false);
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
