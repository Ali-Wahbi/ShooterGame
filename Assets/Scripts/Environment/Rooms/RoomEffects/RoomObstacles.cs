using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObstacles : MonoBehaviour
{

    [SerializeField] List<GameObject> WallObjects, GroundObjects;
    [SerializeField] List<Transform> WallObjectsPos, GroundObjectsPos;
    [SerializeField] float WallObjectsChance, GroundObjectsChance;

    float RandomChance
    {
        get
        {
            return Random.Range(0, 100);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetWallObjects();
        SetGroundObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetWallObjects()
    {
        foreach (Transform item in WallObjectsPos)
        {
            if (RandomChance <= WallObjectsChance)
            {
                InstantiateGameObject(PickRandomObject(WallObjects), item);
            }

        }
    }

    void SetGroundObjects()
    {
        foreach (Transform item in GroundObjectsPos)
        {
            if (RandomChance <= GroundObjectsChance)
            {
                InstantiateGameObject(PickRandomObject(GroundObjects), item);
            }

        }
    }


    void InstantiateGameObject(GameObject obj, Transform pos)
    {
        Instantiate(obj, pos.position, Quaternion.identity, parent: this.transform);
    }

    GameObject PickRandomObject(List<GameObject> list)
    {
        // check for empty List
        if (list.Count == 0) return null;

        int index = Random.Range(0, list.Count);
        return list[index];
    }

}
