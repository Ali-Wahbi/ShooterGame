using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] Sprite WallUp, WallDown, WallLeft, WallRight, WallHorizontal, WallVertical;

    public void SetWallSprite(List<RoomDirection> directions)
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (directions.Count == 1)
        {
            if (directions[0] == RoomDirection.Up)
            {
                sp.sprite = WallUp;
            }
            else if (directions[0] == RoomDirection.Down)
            {
                sp.sprite = WallDown;
            }
            else if (directions[0] == RoomDirection.Left)
            {
                sp.sprite = WallLeft;
            }
            else if (directions[0] == RoomDirection.Right)
            {
                sp.sprite = WallRight;
            }

        }
        else if (directions.Count == 2)
        {
            if (directions[0] == RoomDirection.Up && directions[1] == RoomDirection.Down || directions[0] == RoomDirection.Down && directions[1] == RoomDirection.Up)
            {
                sp.sprite = WallVertical;
            }
            else if (directions[0] == RoomDirection.Left && directions[1] == RoomDirection.Right || directions[0] == RoomDirection.Right && directions[1] == RoomDirection.Left)
            {
                sp.sprite = WallHorizontal;
            }
        }
    }
}
