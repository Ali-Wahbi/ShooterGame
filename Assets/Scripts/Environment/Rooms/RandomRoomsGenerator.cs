using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomsGenerator : MonoBehaviour
{
    [SerializeField] bool AllowRepeatition = false;
    [Space]
    [SerializeField] public List<Transform> RoomsPositions;



    [SerializeField] List<GameObject> AllRooms;
    [SerializeField] GameObject Path;

    [Space]
    [Header("Special Rooms")]

    [SerializeField] GameObject[] SpecialRooms;
    [SerializeField, Tooltip("Number of special rooms to spawn")] int SpecialRoomsCount;

    [Space]
    [Header("Level Organizing")]
    [SerializeField] Biomes biome;
    [SerializeField] RoomDifficulty roomDifficulty;
    [SerializeField] bool UseFolderName = true;
    [SerializeField] string folderName;
    string PreFolder = "Prefabs/Environment/Rooms/";
    [SerializeField] string RoomsFolder;

    HashSet<Vector2> usedPositions = new HashSet<Vector2>();
    PathOrganizer pathOrganizer;
    PositionsOrganizer posOrganizer; // Cache the PositionsOrganizer component

    private void Awake()
    {
        pathOrganizer = GetComponent<PathOrganizer>();
        posOrganizer = GetComponent<PositionsOrganizer>(); // Get the organizer

        if (posOrganizer == null)
        {
            Debug.LogError("PositionsOrganizer component not found on the same GameObject!");
            // Consider adding fallback logic or disabling the script if critical
        }
        if (pathOrganizer == null)
        {
            Debug.LogError("PathOrganizer component not found on the same GameObject!");
            // Consider adding fallback logic or disabling the script if critical
        }

        if (UseFolderName) RoomsFolder = PreFolder + folderName;
        else RoomsFolder = PreFolder + GetRoomFolder();

        // GenerateAll() is moved to Start to allow PositionsOrganizer's Awake to complete.
    }

    private void Start()
    {
        // Ensure dependencies are ready before generating
        if (posOrganizer != null && pathOrganizer != null) GenerateAll();
        else Debug.LogError("Missing required components (PositionsOrganizer or PathOrganizer). Aborting level generation.");
    }
    string GetRoomFolder()
    {
        string result = "";


        switch (biome)
        {
            case Biomes.Lap:
                result = "Lap/";
                break;
            case Biomes.City:
                result = "City/";
                break;
            default:
                break;
        }
        switch (roomDifficulty)
        {
            case RoomDifficulty.Easy:
                result += "Easy/";
                break;
            case RoomDifficulty.Medium:
                result += "Medium/";
                break;
            case RoomDifficulty.Hard:
                result += "Hard/";
                break;
            default:
                break;
        }
        return result;
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
        GenerateAllWalls();
        // GenerateConnectedPaths();
        // GenerateClosedWalls();
    }



    public void GenerateRooms()
    {
        ClearChildren();
        GenerateSpecialRooms();
        GenerateBattleRooms();
        GeneratePaths();
        // GeneratePaths();
    }
    public void GenerateEmptyPaths()
    {
        pathOrganizer.CheckEmpty();
    }

    void GenerateAllWalls()
    {
        StartCoroutine(GenerateWalls());

        IEnumerator GenerateWalls()
        {
            yield return new WaitForSeconds(0.1f);
            GenerateConnectedPaths();
            GenerateClosedWalls();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void GenerateConnectedPaths()
    {
        List<PathsSpawner> paths = pathOrganizer.paths;
        foreach (GameObject path in ConnectedPaths)
        {
            int pathId = ConnectedPaths.IndexOf(path);
            int pathIndex = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i] == null) continue;
                Vector2 start = paths[i].gameObject.transform.position;
                Vector2 end = path.transform.position;
                float distance = Vector2.Distance(start, end);
                if (distance <= 2)
                {
                    pathIndex = i - 10;
                    break;
                }
                else Debug.LogError($"Distance = {distance}"); // test here
            }
            path.GetComponent<PathsSpawner>().PathId = pathId;
            Debug.LogWarning($"At index {pathIndex} set id {pathId}");

            FindAnyObjectByType<MiniMapController>().SetPathId(pathIndex, pathId);
            path.GetComponent<PathsSpawner>().SetPathObject();
        }

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
            int roomId = RoomIdCounter++;
            int roomIndex = RoomsPositions.IndexOf(room);
            FindObjectOfType<MiniMapController>().SetRoomId(roomIndex, roomId);

            if (roomPrefab.GetComponent<RoomController>() == null)
                roomPrefab.AddComponent<RoomController>().SetUpRoom(roomId);
            else
                roomPrefab.GetComponent<RoomController>().SetUpRoom(roomId);

            // Instantiate the room with parent: transform to keep the hierarchy organized in the Unity editor
            Instantiate(roomPrefab, room.position, Quaternion.identity, parent: transform);
        }
    }


    /// <summary>
    /// Generate the paths between the rooms using breadth first algorithm
    /// </summary>
    private void GeneratePaths()
    {

        // Get grid dimensions from PositionsOrganizer, default to 3x3 if unavailable
        int rows = posOrganizer != null ? posOrganizer.rows : 3;
        int cols = posOrganizer != null ? posOrganizer.columns : 3;

        if (RoomsPositions == null || RoomsPositions.Count < rows * cols)
        {
            Debug.LogError($"Not enough RoomPositions ({RoomsPositions?.Count ?? 0}) assigned for a {rows}x{cols} grid. Required: {rows * cols}. Aborting path generation.");
            return;
        }

        bool[,] visited = new bool[rows, cols];
        List<Vector2Int> directions = new List<Vector2Int>
        {
            // Order determines the structure of the resulting spanning tree
            new Vector2Int(0, 1), // Right (Check first)
            new Vector2Int(1, 0), // Down
            new Vector2Int(0, -1), // Left
            new Vector2Int(-1, 0)  // Up   (Check last)
        };

        // Start BFS from the top-left corner (row=0, col=0)
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Vector2Int startNode = new Vector2Int(0, 0);

        // Validate start node is within grid bounds
        if (startNode.x < 0 || startNode.x >= rows || startNode.y < 0 || startNode.y >= cols)
        {
            Debug.LogError($"Start node ({startNode.x},{startNode.y}) is outside the grid dimensions ({rows}x{cols}). Aborting path generation.");
            return;
        }

        // Check if the starting room position is valid before starting BFS
        int startIndex = GetRoomIndex(startNode, cols);
        if (startIndex == -1 || startIndex >= RoomsPositions.Count || RoomsPositions[startIndex] == null)
        {
            Debug.LogError($"Start node ({startNode.x},{startNode.y}) corresponds to an invalid or null RoomPosition (Index: {startIndex}). Aborting path generation.");
            return;
        }

        queue.Enqueue(startNode);
        visited[startNode.x, startNode.y] = true;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // --- Find all valid, unvisited neighbors ---
            List<Vector2Int> validNeighbors = new List<Vector2Int>();
            foreach (var direction in directions)
            {
                Vector2Int neighbor = current + direction;

                // Check if the neighbor is within grid bounds
                if (neighbor.x >= 0 && neighbor.x < rows && neighbor.y >= 0 && neighbor.y < cols)
                {
                    // Check if the neighbor has not been visited yet
                    if (!visited[neighbor.x, neighbor.y])
                    {
                        // Check if the neighbor's room position is valid before proceeding
                        int neighborIndex = GetRoomIndex(neighbor, cols);
                        if (neighborIndex != -1 && neighborIndex < RoomsPositions.Count && RoomsPositions[neighborIndex] != null)
                        {
                            validNeighbors.Add(neighbor);
                        }
                        else
                        {
                            // Log if we skip a potential path due to an invalid target room position
                            Debug.LogWarning($"Skipping potential neighbor {neighbor} from {current}: Target RoomPosition (Index: {neighborIndex}) is invalid or null.");
                        }
                    }
                }
            }

            // --- Shuffle the valid neighbors ---
            // Simple Fisher-Yates shuffle
            for (int i = validNeighbors.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                Vector2Int temp = validNeighbors[i];
                validNeighbors[i] = validNeighbors[j];
                validNeighbors[j] = temp;
            }

            // --- Process shuffled neighbors ---
            foreach (Vector2Int neighbor in validNeighbors)
            {
                // Mark as visited, enqueue, and create the path
                // (The check for visited status is technically redundant here because we filtered earlier,
                // but it's safe to keep in case logic changes)
                if (!visited[neighbor.x, neighbor.y])
                {
                    visited[neighbor.x, neighbor.y] = true;
                    queue.Enqueue(neighbor);
                    CreatePath(current, neighbor); // Connect current to the newly visited neighbor
                }
            }
        }
    }

    [SerializeField] List<GameObject> ConnectedPaths;

    private void CreatePath(Vector2Int from, Vector2Int to)
    {
        // Get column count dynamically for index calculation
        int cols = posOrganizer != null ? posOrganizer.columns : 3;

        int fromIndex = GetRoomIndex(from, cols);
        int toIndex = GetRoomIndex(to, cols);

        // Ensure both indices are valid and within the bounds of the RoomsPositions list
        if (fromIndex != -1 && toIndex != -1 && fromIndex < RoomsPositions.Count && toIndex < RoomsPositions.Count)
        {
            // Double-check that the Transform objects at these indices are not null
            if (RoomsPositions[fromIndex] == null)
            {
                Debug.LogWarning($"Cannot create path from {from}: Source RoomPosition at index {fromIndex} is null.");
                return;
            }
            if (RoomsPositions[toIndex] == null)
            {
                Debug.LogWarning($"Cannot create path to {to}: Target RoomPosition at index {toIndex} is null.");
                return;
            }

            // Find the midpoint between the two rooms
            Vector2 midpoint = (RoomsPositions[fromIndex].position + RoomsPositions[toIndex].position) / 2;

            // Instantiate the path at the midpoint
            GameObject p = Instantiate(Path, midpoint, Quaternion.identity, transform);
            ConnectedPaths.Add(p);
        }
        else
        {
            // Log error if indices are out of list bounds (should be caught by GetRoomIndex or the count check, but good safety)
            if (fromIndex >= RoomsPositions.Count || toIndex >= RoomsPositions.Count)
            {
                Debug.LogError($"Index out of range when creating path between {from} and {to}. FromIndex: {fromIndex}, ToIndex: {toIndex}, RoomsPositions Count: {RoomsPositions.Count}");
            }
            // No error needed if GetRoomIndex returned -1, as that means 'from' or 'to' was outside grid bounds (handled by caller)
        }
    }
    int RoomIdCounter = 1;
    /// <summary>
    /// Generate the special rooms
    /// </summary>
    private void GenerateSpecialRooms()
    {
        usedPositions.Clear(); // clear the used positions list
        SpecialRoomsCount = SpecialRooms.Length; // set the number of special rooms to spawn
        for (int i = 0; i < SpecialRoomsCount;)
        {
            // set at random position in the map
            int ranIndex = Random.Range(0, RoomsPositions.Count);
            if (usedPositions.Contains(RoomsPositions[ranIndex].position)) continue;

            usedPositions.Add(RoomsPositions[ranIndex].position);

            GameObject spRoom = SpecialRooms[SpecialRoomsCount - 1];
            // Check if the special room is null
            if (!spRoom) continue;

            int spRoomId = RoomIdCounter++;
            int spRoomIndex = RoomsPositions.IndexOf(RoomsPositions[ranIndex]);
            FindObjectOfType<MiniMapController>().SetRoomId(spRoomIndex, spRoomId);

            if (spRoom.GetComponent<RoomController>() == null)
                spRoom.AddComponent<RoomController>().SetUpRoom(spRoomId, true);
            else
                spRoom.GetComponent<RoomController>().SetUpRoom(spRoomId, true);

            // Instantiate the special room with parent: transform to keep the hierarchy organized in the Unity editor
            Instantiate(spRoom, RoomsPositions[ranIndex].position, Quaternion.identity, parent: transform);



            i++;
        }
    }

    // Helper function to convert grid coordinates (row, col) to a linear index
    // based on the grid dimensions stored in PositionsOrganizer.
    private int GetRoomIndex(Vector2Int gridPos, int columns)
    {
        // Get row count dynamically
        int rows = posOrganizer != null ? posOrganizer.rows : 3;
        int cols = columns; // Use passed column count

        // Check bounds using dynamic rows/cols
        if (gridPos.x < 0 || gridPos.x >= rows || gridPos.y < 0 || gridPos.y >= cols)
        {
            return -1; // Indicates the grid position is out of bounds
        }
        // Calculate index: rowIndex * numberOfColumns + columnIndex
        return gridPos.x * cols + gridPos.y;
    }

}

public enum Biomes
{
    Lap, City,
}

public enum RoomDifficulty
{
    Easy, Medium, Hard,
}