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
    [SerializeField] List<Image> StaticPaths;
    [SerializeField] CanvasGroup HolderCG;
    RectTransform HolderRC;
    public List<int> AllRoomsIds;
    private void Awake()
    {
        SetUpStartIds();
        HolderRC = HolderCG.GetComponent<RectTransform>();
    }
    void SetUpStartIds()
    {
        for (int i = 0; i < AllRooms.Count; i++)
            AllRoomsIds.Add(0);

    }
    // called by the random room generator
    public void SetRoomId(int index, int id)
    {
        AllRoomsIds[index] = id;
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
        ShowPathInMiniMap(id);
    }

    void ShowRoomInMiniMap(int id, bool isSpecial)
    {
        Image selectedImage = AllRooms[id];
        if (selectedImage.enabled) return;
        if (isSpecial) selectedImage.sprite = SpecielRoom;
        selectedImage.enabled = true;

        TweenSpriteVisibility(selectedImage);
    }
    void ShowPathInMiniMap(int id)
    {
        if (id == -1) return;
        Image selectedImage;
        if (id >= 100)
        {
            id -= 100;
            selectedImage = StaticPaths[id];
        }
        else
            selectedImage = AllPaths[id];


        if (selectedImage.enabled) return;

        selectedImage.enabled = true;

        TweenSpriteVisibility(selectedImage);
    }

    void TweenSpriteVisibility(Image image)
    {
        float startValue = 0f;
        float endValue = 1f;
        DOTween.To(() => startValue, a => image.color = image.color.WithAlpha(a), endValue, 0.7f);

    }

    public bool visible = false;
    public bool hidden = false;
    bool canTweenPos = true;
    bool canTweenVisable = true;
    float Startx = 875f;
    float Endx = 1300f;

    // called by player when presses M
    /// <summary>
    /// Toggles the position of the map
    /// </summary>
    public void ToggleMap()
    {
        if (!canTweenPos) return;
        if (hidden) return;

        if (visible) TweenMapPosX(Endx);
        else TweenMapPosX(Startx);

        visible = !visible;
    }
    [SerializeField] Ease mapEasePos;
    void TweenMapPosX(float end)
    {
        canTweenPos = false;
        HolderRC.DOAnchorPosX(end, 0.7f).SetEase(mapEasePos)
        .OnComplete(() => canTweenPos = true);
    }

    // called by the battle room when battle starts and ends
    /// <summary>
    /// Toggles the visibility of the map using the alpha
    /// </summary>
    public void ToggleMapvisability()
    {
        if (!canTweenVisable) return;

        if (hidden) TweenMapVisibilty(1f);
        else TweenMapVisibilty(0f);

        hidden = !hidden;

    }

    void TweenMapVisibilty(float endAlpha)
    {
        canTweenVisable = false;
        DOTween.To(() => HolderCG.alpha, a => HolderCG.alpha = a, endAlpha, 0.7f)
        .SetEase(Ease.OutCubic)
        .OnComplete(() => canTweenVisable = true);
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
