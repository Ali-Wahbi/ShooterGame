using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawner : MonoBehaviour
{
    [SerializeField] Sprite WallUp, WallDown, WallLeft, WallRight, WallHorizontal, WallVertical;
    [SerializeField] GameObject WallUpGO, WallDownGO, WallLeftGO, WallRightGO, WallHorizontalGO, WallVerticalGO;

    [SerializeField] GameAssets asset;
    public void SetWallSprite(List<RoomDirection> directions)
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();
        if (directions.Count == 1)
        {
            if (directions[0] == RoomDirection.Up)
            {
                // sp.sprite = WallUp;
                InstantiateGameObject(WallUpGO);
            }
            else if (directions[0] == RoomDirection.Down)
            {
                // sp.sprite = WallDown;
                InstantiateGameObject(WallDownGO);
            }
            else if (directions[0] == RoomDirection.Left)
            {
                // sp.sprite = WallLeft;
                InstantiateGameObject(WallLeftGO);
            }
            else if (directions[0] == RoomDirection.Right)
            {
                // sp.sprite = WallRight;
                InstantiateGameObject(WallRightGO);
            }

        }
        else if (directions.Count == 2)
        {
            if (directions[0] == RoomDirection.Up && directions[1] == RoomDirection.Down || directions[0] == RoomDirection.Down && directions[1] == RoomDirection.Up)
            {
                InstantiateGameObject(WallVerticalGO);
                // sp.sprite = WallVertical;
            }
            else if (directions[0] == RoomDirection.Left && directions[1] == RoomDirection.Right || directions[0] == RoomDirection.Right && directions[1] == RoomDirection.Left)
            {
                InstantiateGameObject(WallHorizontalGO);
                // sp.sprite = WallHorizontal;
            }
        }
    }

    void InstantiateGameObject(GameObject _gameObject)
    {
        Instantiate(_gameObject, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    [ContextMenu("Inst Wall Vert")]
    void InstVert() => InstantiateGameObject(WallVerticalGO);

    [ContextMenu("Inst Wall Hori")]
    void InstHori() => InstantiateGameObject(WallHorizontalGO);
}
