using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsSpawner : MonoBehaviour
{

    [SerializeField] Sprite horizontalPath;
    [SerializeField] Sprite verticalPath;
    [SerializeField] GameObject HorizonPath, VericPath;
    [SerializeField] float rayLenght;
    public List<Collider2D> cols;
    // public List<GameObject> pathsCols;
    public List<RoomDirection> rooms;

    SpriteRenderer sp;
    List<Vector2Int> directions;
    Vector2Int up = new Vector2Int(0, 1);
    Vector2Int down = new Vector2Int(0, -1);
    Vector2Int right = new Vector2Int(1, 0);
    Vector2Int left = new Vector2Int(-1, 0);

    public bool isHoriz, isVert;

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        CheckNearbyRooms();
    }


    private void FillDirections()
    {

        if (directions != null) directions.Clear();
        directions = new List<Vector2Int> { up, down, right, left };
    }



    public void CheckNearbyRooms()
    {
        FillDirections();
        foreach (Vector2Int dire in directions)
        {
            //spawn a raycast in that direction
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, dire, rayLenght);
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dire, rayLenght);
            if (hits != null)
            {
                foreach (var hit in hits)
                {

                    //if the raycast hits a room, check if it has a path
                    if (hit.collider.CompareTag("Room"))
                    {
                        cols.Add(hit.collider);
                        if (dire == up || dire == down)
                            SetVerticalPath();
                        else
                            SetHorizontalPath();

                        break;
                    }

                }
            }
        }
    }

    public void CheckPathCollision()
    {
        FillDirections();
        GetComponent<Collider2D>().enabled = false;

        foreach (Vector2Int dire in directions)
        {
            //spawn a raycast in that direction
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dire, 5f);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Path")
                {
                    Destroy(gameObject);
                    break;
                }
            }


        }
    }

    public void CheckClosedWalls()
    {
        FillDirections();
        foreach (Vector2Int dire in directions)
        {
            //spawn a raycast in that direction
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dire, rayLenght);
            if (hits != null)
            {
                foreach (var hit in hits)
                {

                    //if the raycast hits a room, check if it has a path
                    if (hit.collider.CompareTag("Room"))
                    {
                        if (dire == up) rooms.Add(RoomDirection.Up);
                        if (dire == down) rooms.Add(RoomDirection.Down);
                        if (dire == right) rooms.Add(RoomDirection.Right);
                        if (dire == left) rooms.Add(RoomDirection.Left);
                    }

                }
            }
        }

        GetComponent<WallSpawner>().SetWallSprite(rooms);
    }

    private void SetVerticalPath()
    {
        // InstantiateGameObject(_gameObject: VericPath);
        sp.sprite = verticalPath;
        isVert = true;

    }

    private void SetHorizontalPath()
    {
        // InstantiateGameObject(_gameObject: HorizonPath);
        sp.sprite = horizontalPath;
        isHoriz = true;
    }

    [ContextMenu("SetPath")]
    public void SetPathObject()
    {

        if (isHoriz) InstantiateGameObject(_gameObject: HorizonPath);

        if (isVert) InstantiateGameObject(_gameObject: VericPath);

        // Destroy(gameObject);
    }
    public int PathId = 0;
    void InstantiateGameObject(GameObject _gameObject)
    {
        Instantiate(_gameObject, transform.position, Quaternion.identity);
        _gameObject.GetComponent<PathController>().SetUpPath(PathId);

    }
}

public enum RoomDirection
{
    /// <summary>
    /// Represents the directions.
    /// </summary>
    Up, Down, Right, Left
}