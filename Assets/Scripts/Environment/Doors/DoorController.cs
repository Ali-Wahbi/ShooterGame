using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private bool canOpenDoor = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) OpenDoor();// Open the door
        if (Input.GetKeyDown(KeyCode.J)) CloseDoor();// Close the door
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) OpenDoor();// Open the door
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) CloseDoor();// Close the door
    }


    void OpenDoor()
    {
        if (canOpenDoor)
        {
            doorAnimator.SetTrigger("PlayOpen");
            doorCollider.enabled = false;
        }
    }

    void CloseDoor()
    {
        if (canOpenDoor)
        {
            doorAnimator.SetTrigger("PlayShut");
            doorCollider.enabled = true;
        }
    }

    // called by the room which the door is open in
    public void SetCanOpenDoor(bool canOpen) => canOpenDoor = canOpen;
}
