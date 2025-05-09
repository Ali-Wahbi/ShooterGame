using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{

    public int PathId = 0;

    public void OnPlayerEnterPath()
    {
        // send to the mini map that the player entered Path with Id PathId
        FindObjectOfType<MiniMapController>().ShowPathInMap(PathId);
    }
    public void SetUpPath(int id)
    {
        PathId = id;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            OnPlayerEnterPath();
    }
}
