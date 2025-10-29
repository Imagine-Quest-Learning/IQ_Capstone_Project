using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script to "unhide" lose screen
public class LoseScreenController : MonoBehaviour
{
    public void ActivateScreen(){
        gameObject.SetActive(true);
    }
}
