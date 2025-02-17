using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomsGenerator : MonoBehaviour
{
    [SerializeField] bool AllowRepeatition = false;
    [Space]
    [SerializeField] List<Transform> RoomsPositions;



    [SerializeField] List<GameObject> AllRooms;
    [SerializeField] GameObject Path;
    [SerializeField] GameObject SpecialRoom;
    [SerializeField, Tooltip("Number of special rooms to spawn")] int SpecialRoomCount;

    [Space]
    [SerializeField] string folderName;
    string PreFolder = "Prefabs/Environment/Rooms/";
    string RoomsFolder;

    HashSet<Vector2> usedPositions = new HashSet<Vector2>();
    PathOrganizer pathOrganizer;
    private void Start()
    {
        pathOrganizer = GetComponent<PathOrganizer>();
        RoomsFolder = PreFolder + folderName;

        GenerateAll();
    }

    private void FillAllRooms()
    {
        AllRooms.Clear();
        GameObject[] arr = Resources.LoadAll<GameObject>(RoomsFolder);
        AllRooms = ListRooms(arr);
    }

    List<GameObject> ListRooms(GameObject[] arr)
    {
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject obj in arr)
        {
            list.Add(obj);
        }
        return list;
    }

    public void GenerateAll()
    {
        GenerateRooms();
        GenerateEmptyPaths();
        GenerateClosedWalls();
    }
    public void GenerateRooms()
    {
        ClearChildren();
        GenerateSpecialRooms();
        GenerateBattleRooms();
        GeneratePaths();
    }
    public void GenerateEmptyPaths()
    {
        pathOrganizer.CheckEmpty();
    }

    public void GenerateClosedWalls()
    {
        pathOrganizer.CheckClosedWalls();
    }

    /// <summary>
    /// Clear all children of the current object
    /// </summary> 
    private void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    /// <summary>
    /// Generate the battle rooms from the battle rooms folder
    /// </summary>
    private void GenerateBattleRooms()
    {
        FillAllRooms();

        foreach (Transform room in RoomsPositions)
        {
            if (usedPositions.Contains(room.position)) continue; // skip the position if it's already used
            if (AllRooms.Count == 0) break;
            int ranIndex = Random.Range(0, AllRooms.Count);
            GameObject roomPrefab = AllRooms[ranIndex];
            if (!AllowRepeatition)
            {
                // remove the room from the list of available rooms
                AllRooms.RemoveAt(ranIndex);
            }

            // Instantiate the room with parent: transform to keep the hierarchy organized in the Unity editor
            Instantiate(roomPrefab, room.position, Quaternion.identity, parent: transform);
        }
    }


    /// <summary>
    /// Generate the paths between the rooms using breadth first algorithm
    /// </summary>
    private void GeneratePaths()
    {
        int gridSize = 3;
        bool[,] visited = new bool[gridSize, gridSize];
        List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0, 1), // Right
        new Vector2Int(1, 0), // Down
        new Vector2Int(0, -1), // Left
        new Vector2Int(-1, 0) // Up
    };

        // Start from the top-left corner
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(0, 0));
        visited[0, 0] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            List<Vector2Int> neighbors = new List<Vector2Int>();

            foreach (var direction in directions)
            {
                Vector2Int neighbor = current + direction;
                if (neighbor.x >= 0 && neighbor.x < gridSize && neighbor.y >= 0 && neighbor.y < gridSize && !visited[neighbor.x, neighbor.y])
                {
                    neighbors.Add(neighbor);
                }
            }

            if (neighbors.Count > 0)
            {
                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                visited[chosenNeighbor.x, chosenNeighbor.y] = true;
                queue.Enqueue(chosenNeighbor);

                // Create a path between current and chosenNeighbor
                CreatePath(current, chosenNeighbor);
            }
        }
    }

    private void CreatePath(Vector2Int from, Vector2Int to)
    {
        int gridSize = 3;
        int fromIndex = from.x * gridSize + from.y;
        int toIndex = to.x * gridSize + to.y;

        if (fromIndex >= 0 && fromIndex < RoomsPositions.Count && toIndex >= 0 && toIndex < RoomsPositions.Count)
        {
            // Find the midpoint between the two rooms
            Vector2 midpoint = (RoomsPositions[fromIndex].position + RoomsPositions[toIndex].position) / 2;

            // Instantiate the path at the midpoint
            Instantiate(Path, midpoint, Quaternion.identity, transform);


        }
        else
        {
            Debug.LogError("Index out of range: fromIndex or toIndex is out of bounds.");
        }
    }

    /// <summary>
    /// Generate the special rooms
    /// </summary>
    private void GenerateSpecialRooms()
    {
        usedPositions.Clear(); // clear the used positions list
        for (int i = 0; i < SpecialRoomCount;)
        {
            int ranIndex = Random.Range(0, RoomsPositions.Count);
            if (usedPositions.Contains(RoomsPositions[ranIndex].position)) continue;

            usedPositions.Add(RoomsPositions[ranIndex].position);

            Instantiate(SpecialRoom, RoomsPositions[ranIndex].position, Quaternion.identity, parent: transform);
            i++;
        }
    }


}
