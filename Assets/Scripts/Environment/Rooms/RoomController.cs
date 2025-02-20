using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] BoxCollider2D bColldier;
    public void DisableCollider()
    {
        bColldier.enabled = false;
    }
}
