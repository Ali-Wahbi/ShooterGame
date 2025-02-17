using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsSpawner : MonoBehaviour
{

    [SerializeField] Sprite horizontalPath;
    [SerializeField] Sprite verticalPath;
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
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dire, 0.2f);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject.tag == "Path")
                {
                    Destroy(gameObject);
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
        sp.sprite = verticalPath;
    }

    private void SetHorizontalPath()
    {
        sp.sprite = horizontalPath;
    }
}

public enum RoomDirection
{
    /// <summary>
    /// Represents the directions.
    /// </summary>
    Up, Down, Right, Left
}