using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    private List<GameObject> barrels = new List<GameObject>();
    private int remainingBarrels = 3;

    public void Awake()
    {
        //make sure list is empty
        barrels.Clear();

        //for each child object in the hiearchy (aka barrels) add them to the list
        foreach (Transform child in transform)
        {
            barrels.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }

    public void Initialize()
    {
        foreach (GameObject barrel in barrels)
        {
            if (barrel != null) barrel.SetActive(true);
        }
        remainingBarrels = 3;
    }

    public void HideAllBarrels()
    {
        foreach (GameObject barrel in barrels)
        {
            if (barrel != null) barrel.SetActive(false);
        }
    }

    public void DestroyBarrel(GameObject barrel)
    {
        if (barrel != null)
        {
            remainingBarrels--;
            barrel.SetActive(false);

            if (remainingBarrels == 0)
            {
                SubtractionGameManager.Instance.OnPlayerLost();
            }
        }
    }
    
    public int GetRemainingBarrels()
    {
        return barrels.Count;
    }
}
