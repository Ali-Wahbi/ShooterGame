using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] Sprite SpecielRoom;
    [SerializeField] List<Image> AllRooms;
    [SerializeField] List<Image> AllPaths;
    public List<int> AllRoomsIds;
    public List<int> AllPathsIds;
    private void Awake()
    {
        SetUpStartIds();
    }
    void SetUpStartIds()
    {
        for (int i = 0; i < AllRooms.Count; i++)
            AllRoomsIds.Add(0);

        for (int i = 0; i < AllPaths.Count; i++)
            AllPathsIds.Add(0);
    }
    // called by the random room generator
    public void SetRoomId(int index, int id)
    {
        AllRoomsIds[index] = id;
    }
    public void SetPathId(int index, int id)
    {
        AllPathsIds[index] = id;
    }

    // called when the player enters a room
    public void ShowRoomInMap(int id, bool isSpecial)
    {
        for (int i = 0; i < AllRoomsIds.Count; i++)
        {
            if (AllRoomsIds[i] == id)
            {
                Debug.Log($"player entered roomId {id}.");
                ShowRoomInMiniMap(i, isSpecial);
                break;
            }
        }
    }

    public void ShowPathInMap(int id)
    {
        for (int i = 0; i < AllPathsIds.Count; i++)
        {
            if (AllPathsIds[i] == id)
            {
                Debug.Log($"player entered pathId {id}.");
                ShowPathInMiniMap(i);
                break;
            }
        }
    }

    void ShowRoomInMiniMap(int id, bool isSpecial)
    {
        Image selectedImage = AllRooms[id];

        if (isSpecial) selectedImage.sprite = SpecielRoom;
        selectedImage.enabled = true;

        TweenSpriteVisibility(selectedImage);
    }
    void ShowPathInMiniMap(int id)
    {
        Image selectedImage = AllPaths[id];

        selectedImage.enabled = true;

        TweenSpriteVisibility(selectedImage);
    }

    void TweenSpriteVisibility(Image image)
    {
        float startValue = 0f;
        float endValue = 1f;
        DOTween.To(() => startValue, a => image.color = image.color.WithAlpha(a), endValue, 0.7f);

    }

    void DisableMiniMapSprites()
    {
        foreach (Image room in AllRooms)
        {
            room.enabled = false;
        }

        foreach (Image path in AllPaths)
        {
            path.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableMiniMapSprites();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
