using System.Collections;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public RoomDoorController doorController;

    private bool hasEntered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasEntered && collision.CompareTag("Player"))
        {
            hasEntered = true;
           
            StartCoroutine(DelayedOpenDoor());

            
            doorController.CloseDoor();
        }
    }

    IEnumerator DelayedOpenDoor()
    {
        yield return new WaitForSeconds(3f);
        doorController.OpenDoor();
    }
}
