using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    private List<GameObject> barrels = new List<GameObject>();

    public void Initialize()
    {
        //make sure list is empty
        barrels.Clear();

        //for each child object in the hiearchy (aka barrels) add them to the list
        foreach (Transform child in transform)
        {
            barrels.Add(child.gameObject);
        }
    }

    public void DestroyBarrel(GameObject barrel)
    {
        if (barrel != null)
        {
            barrels.Remove(barrel);
            Destroy(barrel);

            if (barrels.Count == 0)
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
