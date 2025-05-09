using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOrganizer : MonoBehaviour
{
    [SerializeField]
    public List<PathsSpawner> paths;
    public void SetPathsIndex()
    {
        for (int i = 10; i < paths.Count; i++)
        {
            paths[i].SetUpPath(i - 10);
        }
    }

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

    public int GetPathIndex(PathsSpawner path)
    {
        if (paths.Contains(path))
            return paths.IndexOf(path);
        else return 0;
    }
}
