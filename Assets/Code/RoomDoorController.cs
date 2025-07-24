using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDoorController : MonoBehaviour
{
    public Tilemap openDoorTilemap;     // Cửa mở
    public Tilemap closeDoorTilemap;    // Cửa đóng

    private bool doorClosed = false;

    private void Start()
    {
        openDoorTilemap.gameObject.SetActive(true);
        closeDoorTilemap.gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        if (!doorClosed)
        {
            doorClosed = true;
            openDoorTilemap.gameObject.SetActive(false);
            closeDoorTilemap.gameObject.SetActive(true);
        }
    }

    public void OpenDoor()
    {
        closeDoorTilemap.gameObject.SetActive(false);
        openDoorTilemap.gameObject.SetActive(true);
        doorClosed = false;
    }
}
