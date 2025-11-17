using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Description: When the player reaches the AddroomOut scene,
                 mark this math room as completed in the GameManager.
*/
public class AdditionRoomWinMarker : MonoBehaviour
{
    [SerializeField] string roomName = "AddRoom";

    void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkRoomComplete(roomName);
        }
    }
}
