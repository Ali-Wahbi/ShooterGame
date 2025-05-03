using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject powerDisplayPrefab;
    [SerializeField] private GameObject powerDisplayContainer;


    private static PowerDisplayer _pd;

    public static PowerDisplayer Pd
    {
        get
        {
            if (_pd == null)
            {
                _pd = Instantiate(Resources.Load<PowerDisplayer>("Prefabs/Environment/Rooms/StaticRooms/PowersUI"));
            }
            return _pd;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneChangeManager.Scm.AddToRetryEvents(ResetDisplay);
        SceneChangeManager.Scm.AddToQuitEvents(ResetDisplay);
    }
    public HashSet<Sprite> PowerSprites;
    public HashSet<Power> PowersSet;

    public void AddPower(Power power)
    {
        if (PowersSet == null) PowersSet = new HashSet<Power>();
        PowersSet.Add(power);
        AddPowerSprite(power.PowerOutlinedIcon);
    }
    void AddPowerSprite(Sprite sprite)
    {
        // add the sprite to save between levels
        if (PowerSprites == null) PowerSprites = new HashSet<Sprite>();
        PowerSprites.Add(sprite);


        GameObject childHolder = new GameObject(sprite.name + "_holder", typeof(RectTransform));
        GameObject child = new GameObject(sprite.name);

        childHolder.transform.SetParent(powerDisplayContainer.transform);
        child.transform.SetParent(childHolder.transform);


        child.AddComponent<Image>();
        child.GetComponent<Image>().sprite = sprite;
        TweenChildAppearance(child.GetComponent<Image>(), (RectTransform)child.transform);

    }

    float StartPosY = 40.0f, EndPosY = 0.0f;
    void TweenChildAppearance(Image childImage, RectTransform childPos)
    {
        //using DoTween
        float duration = 0.7f;

        DOTween.To(() => StartPosY, y => childPos.anchoredPosition = childPos.anchoredPosition.With(y: y), EndPosY, duration).SetEase(Ease.OutSine);
        DOTween.To(() => 0f, x => SetImageAlpha(childImage, x), 1.0f, duration * 0.85f).SetEase(Ease.OutSine);

    }
    void SetImageAlpha(Image image, float alpha)
    {
        Color temp = image.color;
        temp.a = alpha;
        image.color = temp;
    }

    public int GetPowersCount()
    {

        if (PowersSet == null) PowersSet = new HashSet<Power>();
        return PowersSet.Count;
    }

    public HashSet<Sprite> GetPowerSprites()
    {
        if (PowerSprites == null) PowerSprites = new HashSet<Sprite>();
        return PowerSprites;
    }


    public HashSet<Power> GetPowersSet()
    {
        if (PowersSet == null) PowersSet = new HashSet<Power>();
        return PowersSet;
    }


    public void ResetDisplay()
    {
        // remove all children from the display
        // foreach (Transform child in transform)
        // {
        //     Destroy(child.gameObject);
        // }

        Destroy(gameObject);
    }

}
