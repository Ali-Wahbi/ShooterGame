using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionsOrganizer : MonoBehaviour
{
    [SerializeField] bool workOnAwake = true; // Flag to control if this script should run on Awake or Start
    public List<Transform> RoomsPositions;
    List<Vector2> RoomsPositionsVector2;
    public List<Transform> paths;
    public List<Vector2> PathsPositionsVector2;

    [SerializeField] float x_Width, y_Height;
    [SerializeField] int rows = 3, columns = 3;



    void Awake()
    {
        if (!workOnAwake) return; // Skip if not set to work on Awake
        // Initialize lists
        RoomsPositionsVector2 = new List<Vector2>();
        PathsPositionsVector2 = new List<Vector2>();

        // Get references from components on the same GameObject
        // Ensure these components exist and have populated their lists if needed before Awake runs.
        // If RandomRoomsGenerator/PathOrganizer populate lists in their Awake, consider execution order or moving this logic to Start.
        RandomRoomsGenerator roomGenerator = GetComponent<RandomRoomsGenerator>();
        PathOrganizer pathOrganizer = GetComponent<PathOrganizer>();

        if (roomGenerator == null || pathOrganizer == null)
        {
            Debug.LogError("Missing RandomRoomsGenerator or PathOrganizer component!");
            return; // Stop execution if components are missing
        }

        RoomsPositions = roomGenerator.RoomsPositions;
        paths = PathToTransformList(pathOrganizer.paths); // Convert PathSpawners to Transforms

        // Check if required lists are populated
        if (RoomsPositions == null || RoomsPositions.Count == 0)
        {
            Debug.LogError("RoomsPositions list is null or empty. Ensure RandomRoomsGenerator populates it.");
            // It might be okay if it's populated later (e.g., in Start), but Awake positioning depends on it.
        }
        if (paths == null) // paths list itself could be null if PathToTransformList fails or input is null
        {
            Debug.LogError("Paths list is null. Ensure PathOrganizer has paths.");
            paths = new List<Transform>(); // Initialize to prevent null reference errors later
        }


        // Set positions
        SetRoomPositions();
        SetPathPositions();
    }

    // Helper to convert List<PathsSpawner> to List<Transform>
    List<Transform> PathToTransformList(List<PathsSpawner> pathSpawners)
    {
        List<Transform> result = new List<Transform>();
        if (pathSpawners == null)
        {
            Debug.LogWarning("Input pathSpawners list is null in PathToTransformList.");
            return result; // Return empty list
        }
        foreach (PathsSpawner path in pathSpawners)
        {
            if (path != null) // Check if the spawner itself is not null
            {
                result.Add(path.transform);
            }
            else
            {
                Debug.LogWarning("Null PathsSpawner found in the list.");
            }
        }
        foreach (Transform t in paths)
        {
            result.Add(t);
        }
        return result;
    }

    // Sets the positions of the room transforms based on grid parameters
    void SetRoomPositions()
    {
        RoomsPositionsVector2.Clear(); // Clear previous positions

        // Ensure we have enough transforms in the list
        int requiredRoomCount = rows * columns;
        while (RoomsPositions.Count < requiredRoomCount)
        {
            // If not enough transforms are assigned in the inspector, create placeholders
            // This might indicate a setup issue, so a warning is good.
            Debug.LogWarning($"Not enough Room Transforms ({RoomsPositions.Count}) for a {rows}x{columns} grid ({requiredRoomCount}). Creating placeholders.");
            GameObject placeholder = new GameObject($"RoomPlaceholder_{RoomsPositions.Count}");
            placeholder.transform.SetParent(this.transform); // Keep hierarchy clean
            RoomsPositions.Add(placeholder.transform);
            // Note: These placeholders won't have room components unless added elsewhere.
        }


        int roomIndex = 0;
        for (int i = 0; i < rows; i++) // 'i' represents row index
        {
            for (int j = 0; j < columns; j++) // 'j' represents column index
            {
                // Calculate position: (columnIndex * width, rowIndex * height)
                Vector2 pos = new Vector2(j * x_Width, i * y_Height);
                RoomsPositionsVector2.Add(pos);

                // Assign position to the corresponding transform
                if (roomIndex < RoomsPositions.Count && RoomsPositions[roomIndex] != null)
                {
                    RoomsPositions[roomIndex].position = pos;
                }
                else if (roomIndex >= RoomsPositions.Count)
                {
                    Debug.LogError($"Attempting to access RoomPosition index {roomIndex}, but list only has {RoomsPositions.Count} elements.");
                    // This shouldn't happen with the check above, but good for safety.
                }
                else // RoomsPositions[roomIndex] == null
                {
                    Debug.LogWarning($"Room Transform at index {roomIndex} is null.");
                }
                roomIndex++;
            }
        }
    }

    // Main method to set path positions (renamed from SetPathPositios)
    void SetPathPositions()
    {
        SetPathsInBetweenRooms(); // Corrected spelling
    }

    void SetPathsInBetweenRooms()
    {
        float startX = RoomsPositions[0].position.x - x_Width / 2; // Assuming first room is at (0,0)
        float startY = RoomsPositions[0].position.y - y_Height / 2; // Assuming first room is at (0,0)
        PathsPositionsVector2.Clear(); // Clear previous calculated positions

        int columnsLoops;
        float rowStartx;
        for (int rows = 0; rows < this.rows * 2 + 1; rows++)
        {
            if (rows % 2 == 0)
            {
                columnsLoops = 3;
                rowStartx = startX + x_Width / 2;
            }
            else
            {
                rowStartx = startX;
                columnsLoops = 4;
            }

            for (int col = 0; col < columnsLoops; col++)
            {
                Vector2 pos = new Vector2(rowStartx + col * x_Width, startY + rows * y_Height / 2);
                PathsPositionsVector2.Add(pos);
            }
        }

        for (int path = 0; path < paths.Count; path++)
        {
            paths[path].position = PathsPositionsVector2[path];
        }
    }

    // Calculates and sets positions for paths between rooms
    // void SetPathsInBetweenRooms() // Corrected spelling
    // {
    //     PathsPositionsVector2.Clear(); // Clear previous calculated positions
    //     int pathIndex = 0; // Index for the 'paths' list

    //     // --- Horizontal Paths ---
    //     // Iterate through each row
    //     for (int r = 0; r < rows; r++)
    //     {
    //         // Iterate through columns where a horizontal path can start (up to second-to-last column)
    //         for (int c = 0; c < columns - 1; c++)
    //         {
    //             // Calculate the position halfway between room (r, c) and room (r, c+1)
    //             // Room (r, c) is at (c * x_Width, r * y_Height)
    //             // Room (r, c+1) is at ((c+1) * x_Width, r * y_Height)
    //             float pathX = c * x_Width + x_Width / 2.0f;
    //             float pathY = r * y_Height;
    //             Vector2 pathPos = new Vector2(pathX, pathY);

    //             PathsPositionsVector2.Add(pathPos); // Store the calculated position

    //             // Assign the position to the next available path Transform
    //             if (pathIndex < paths.Count && paths[pathIndex] != null)
    //             {
    //                 paths[pathIndex].position = pathPos;
    //                 pathIndex++;
    //             }
    //             else
    //             {
    //                 // Log warning if we run out of path transforms or encounter a null one
    //                 if (pathIndex >= paths.Count)
    //                     Debug.LogWarning($"Not enough path Transforms provided ({paths.Count}) for all connections. Needed more for horizontal path at ({r},{c}).");
    //                 else
    //                     Debug.LogWarning($"Path Transform at index {pathIndex} is null.");
    //                 // Decide if you want to 'return' or 'break' if this is critical
    //             }
    //         }
    //     }

    //     // --- Vertical Paths ---
    //     // Iterate through each column
    //     for (int c = 0; c < columns; c++)
    //     {
    //         // Iterate through rows where a vertical path can start (up to second-to-last row)
    //         for (int r = 0; r < rows - 1; r++)
    //         {
    //             // Calculate the position halfway between room (r, c) and room (r+1, c)
    //             // Room (r, c) is at (c * x_Width, r * y_Height)
    //             // Room (r+1, c) is at (c * x_Width, (r+1) * y_Height)
    //             float pathX = c * x_Width;
    //             float pathY = r * y_Height + y_Height / 2.0f;
    //             Vector2 pathPos = new Vector2(pathX, pathY);

    //             PathsPositionsVector2.Add(pathPos); // Store the calculated position

    //             // Assign the position to the next available path Transform
    //             if (pathIndex < paths.Count && paths[pathIndex] != null)
    //             {
    //                 paths[pathIndex].position = pathPos;
    //                 pathIndex++;
    //             }
    //             else
    //             {
    //                 // Log warning if we run out of path transforms or encounter a null one
    //                 if (pathIndex >= paths.Count)
    //                     Debug.LogWarning($"Not enough path Transforms provided ({paths.Count}) for all connections. Needed more for vertical path at ({r},{c}).");
    //                 else
    //                     Debug.LogWarning($"Path Transform at index {pathIndex} is null.");
    //                 // Decide if you want to 'return' or 'break' if this is critical
    //             }
    //         }
    //     }

    //     // Optional: Check if the number of placed paths matches the expected number
    //     int expectedPaths = (rows * (columns - 1)) + (columns * (rows - 1));
    //     if (pathIndex != expectedPaths && paths.Count >= expectedPaths)
    //     {
    //         Debug.LogWarning($"Placed {pathIndex} paths, but expected {expectedPaths} for a {rows}x{columns} grid. Check logic or path list.");
    //     }
    //     else if (paths.Count < expectedPaths)
    //     {
    //         Debug.LogWarning($"Provided {paths.Count} path transforms, but {expectedPaths} are needed for a {rows}x{columns} grid.");
    //     }
    // }

    // Start, Update, etc. are not used in this snippet but kept for structure
    void Start()
    {
        // If positioning logic needs to happen after other Awakes, move it here from Awake.
    }

    void Update()
    {
        // Usually not needed for initial setup
    }
}

