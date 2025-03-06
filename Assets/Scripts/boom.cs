using UnityEngine;

public class boom : MonoBehaviour
{
    void Start()
    {
        // Destroy this GameObject after 1 second
        Destroy(this.gameObject, 1f);
    }

}
