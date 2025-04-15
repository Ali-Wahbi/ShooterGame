using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOrganizer : MonoBehaviour
{
    [SerializeField]
    public List<PathsSpawner> paths;
    // Start is called before the first frame update
    public void CheckEmpty()
    {
        foreach (PathsSpawner path in paths)
        {
            if (path) path.CheckPathCollision();
        }
    }

    public void CheckClosedWalls()
    {
        foreach (PathsSpawner path in paths)
        {
            if (path) path.CheckClosedWalls();
        }
    }
}
