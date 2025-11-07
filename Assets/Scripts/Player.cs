using UnityEngine;

public class Player : MonoBehaviour
{
    /* Static reference to Player object 
        - allows us to reference it from all scenes 
        - Need this because it is a persistent object (aka part of Don'tDestroyOnLoad)
    */
    public static Player Instance { get; private set; } //reference by Player.Instance....

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        transform.position += movement * Time.deltaTime;
    }

}
