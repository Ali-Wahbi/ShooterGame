using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private int nextRoomIndex;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // replace by the elevator animation
            // Load the next level
            GoToNextLevel();
        }
    }

    void GoToNextLevel()
    {
        // Load the next level
        SceneChangeManager.Scm.LoadLevel(nextRoomIndex);
    }
}
