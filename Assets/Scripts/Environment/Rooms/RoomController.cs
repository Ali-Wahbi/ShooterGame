using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{

    public int RoomId = 0;
    public bool IsSpecialRoom = false;
    public void OnPlayerEnterRoom()
    {
        // send to the mini map that the player entered room with Id RoomId
        FindObjectOfType<MiniMapController>().ShowRoomInMap(RoomId, IsSpecialRoom);
        if (!IsSpecialRoom) FindObjectOfType<MiniMapController>().ToggleMapvisability();
    }
    public void SetUpRoom(int id, bool isSpecial = false)
    {
        RoomId = id;
        IsSpecialRoom = isSpecial;
    }


}
