using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Exentsions;

public class InfoDisplayer : MonoBehaviour
{
    [SerializeField] float PosMinimum = -0.75f, PosMaximum = -0.25f;
    [SerializeField] Ease TweenEase;
    [SerializeField] Image BG;
    [Header("WASD")]
    [SerializeField] List<Image> WASD;
    [SerializeField] TextMeshProUGUI WasdInst;

    [Header("Mouse clicks")]
    [SerializeField] Image Mouse1;
    [SerializeField] Image Mouse2;
    [SerializeField] Sprite click1, click2, empty;
    [SerializeField] TextMeshProUGUI Click1Inst, Click2Inst;

    [Header("Space")]
    [SerializeField] Image Space;
    [SerializeField] TextMeshProUGUI SpaceInst;

    [Header("MiniMap")]
    [SerializeField] Image MiniMapImage;
    [SerializeField] TextMeshProUGUI MiniMapInst;

    [Header("Hide Info")]
    [SerializeField] Image HideInfoImage;
    [SerializeField] TextMeshProUGUI HideInfoInst;



    CanvasGroup Cg;
    #region Background UI

    float selectedPosX
    {
        set
        {
            BG.material.SetFloat("_XPos", value);
        }
    }
    float selectedPosY
    {
        set
        {
            BG.material.SetFloat("_YPos", value);
        }
    }


    float BgPanelValue
    {
        set
        {
            BG.material.SetFloat("_Value", value);
        }
    }
    float getRandomPos()
    {
        return Random.Range(PosMinimum, PosMaximum);
    }

    public bool canToggle = true;
    bool isShown = false;
    public void ToggleInfoDisplayer()
    {
        if (!canToggle) return;
        canToggle = false;

        if (isShown) HideInfoDisplayer();
        else ShowInfoDisplayer();

        isShown = !isShown;
    }

    void ShowInfoDisplayer()
    {
        // randomize the position
        selectedPosX = getRandomPos();
        selectedPosY = getRandomPos();
        DisableAllUI();
        Cg.alpha = 0f;
        Cg.blocksRaycasts = true;
        Cg.DOFade(1f, 0.3f).OnComplete(() => { Debug.Log($"Info Displayer Alpha = {Cg.alpha}"); });
        TweenPanelVisible(onComplete: () => TweenInfoOnScreen(), startValue: 0f, endValue: 1f);
    }
    void TweenInfoOnScreen()
    {
        ShowWASDInfo();
        ShowMouseInfo();
        ShowSpaceInfo();
        ShowMiniMapInfo();
        ShowHideInfoUI(addDelay: 0.9f);
    }

    void HideInfoDisplayer()
    {
        Cg.alpha = 1;
        Cg.blocksRaycasts = false;
        canAnimateMouse = false;
        Cg.DOFade(0f, 0.5f).OnComplete(() => canToggle = true);
        TweenPanelVisible(startValue: 1f, endValue: 0f, duration: 0.45f);

    }
    #endregion
    #region UI Tweens

    [Header("Eases")]
    public Ease AlphaEase;
    public Ease PosEase;
    public Ease ScaleEase;
    void ShowWASDInfo(float addDelay = 0f)
    {

        for (int i = 0; i < WASD.Count; i++)
        {
            TweenImageAlpha(WASD[i], delay: (i * 0.3f) + addDelay);
            TweenImagePosY(WASD[i].rectTransform, delay: (i * 0.3f) + addDelay);

        }
        float InstDelay = (WASD.Count * 0.3f) + addDelay;
        // float InstDelay = 0f;
        TweenTextAlpha(text: WasdInst, delay: InstDelay);
        TweenImagePosY(rect: WasdInst.rectTransform, delay: InstDelay * 0.3f, Yadd: 150f);


    }

    void ShowMouseInfo(float addDelay = 0f)
    {
        TweenImageAlpha(Mouse1, 0f + addDelay, duration: 1.25f);
        TweenImageAlpha(Mouse2, 0f + addDelay, duration: 1.25f);

        TweenImageScale(Mouse1.rectTransform, delay: 0.5f + addDelay);
        TweenImageScale(Mouse2.rectTransform, delay: 0.5f + addDelay);


        float InstDelay = 1.25f;

        TweenTextAlpha(Click1Inst, delay: InstDelay);
        TweenTextAlpha(Click2Inst, delay: InstDelay);

        TweenImagePosY(Click1Inst.rectTransform, delay: InstDelay, Yadd: 150f);
        TweenImagePosY(Click2Inst.rectTransform, delay: InstDelay, Yadd: 150f, OnComplete: () => StartMouseAnimations());
    }

    void ShowSpaceInfo(float addDelay = 0f)
    {
        TweenImageAlpha(Space, 0f + addDelay, duration: 1.25f);
        TweenImagePosY(Space.rectTransform, delay: 0.5f + addDelay, Yadd: 50f);

        float InstDelay = 1.25f + addDelay;
        TweenTextAlpha(text: SpaceInst, delay: InstDelay);
        TweenImagePosY(rect: SpaceInst.rectTransform, delay: InstDelay, Yadd: 150f);
    }
    void ShowMiniMapInfo(float addDelay = 0f)
    {
        TweenImageAlpha(MiniMapImage, 0f + addDelay, duration: 1.25f);
        TweenImagePosY(MiniMapImage.rectTransform, delay: 0.5f + addDelay, Yadd: 50f);

        float InstDelay = 1.25f + addDelay;
        TweenTextAlpha(text: MiniMapInst, delay: InstDelay);
        TweenImagePosY(rect: MiniMapInst.rectTransform, delay: InstDelay, Yadd: 150f);
    }
    void ShowHideInfoUI(float addDelay = 0f)
    {
        //TODO: Impelement this
        TweenImageAlpha(HideInfoImage, 0f + addDelay, duration: 1.25f);
        TweenImagePosY(HideInfoImage.rectTransform, delay: 0.5f + addDelay, Yadd: 150f);

        float InstDelay = 0.25f + addDelay;
        TweenTextAlpha(text: HideInfoInst, delay: InstDelay);
        TweenImagePosY(rect: HideInfoInst.rectTransform, delay: InstDelay, Yadd: 150f);
    }
    #endregion
    #region Tweens Helpers
    void TweenImageAlpha(Image image, float delay, float duration = 0.75f)
    {

        image.color = image.color.WithAlpha(0f);
        image.DOFade(1f, duration)
        .SetEase(AlphaEase).SetDelay(delay);
    }
    void TweenTextAlpha(TextMeshProUGUI text, float delay)
    {
        text.color = text.color.WithAlpha(0f);
        text.DOFade(1f, 0.5f)
        .SetEase(AlphaEase).SetDelay(delay);
    }

    void TweenImagePosY(RectTransform rect, float delay, float Yadd = 20f, TweenCallback OnComplete = null)
    {
        float EndValue = rect.anchoredPosition.y;
        rect.anchoredPosition = rect.anchoredPosition.Add(y: Yadd);
        rect.DOAnchorPosY(EndValue, delay).SetEase(PosEase).OnComplete(OnComplete);
    }
    void TweenImagePosX(RectTransform rect, float delay, float Xadd = 20f, TweenCallback OnComplete = null)
    {
        float EndValue = rect.anchoredPosition.y;
        rect.anchoredPosition = rect.anchoredPosition.Add(y: Xadd);
        rect.DOAnchorPosX(EndValue, delay).SetEase(PosEase).OnComplete(() => OnComplete());
    }

    void TweenImageScale(RectTransform rect, float delay, float startValue = 0.75f, float endValue = 1f)
    {
        rect.localScale = new Vector3(startValue, startValue, startValue);

        rect.DOScale(endValue, delay).SetEase(ScaleEase).SetDelay(delay);
    }
    private void TweenPanelVisible(TweenCallback onComplete = null, float startValue = 1f, float endValue = 0f, float duration = 1f)
    {
        DOTween.To(() => startValue, x => BgPanelValue = x, endValue, duration)
        .SetEase(TweenEase)
        .OnComplete(() => { canToggle = true; onComplete(); });
    }


    #endregion

    #region Setup Helpers
    bool canAnimateMouse = false;
    void StartMouseAnimations()
    {
        canAnimateMouse = true;
        StartCoroutine(AnimateMouseImage(Mouse1, click1));
        StartCoroutine(AnimateMouseImage(Mouse2, click2));
    }

    float delay = 1;
    IEnumerator AnimateMouseImage(Image mouse, Sprite toImage = null)
    {
        if (canAnimateMouse)
        {
            yield return new WaitForSeconds(delay);
            ChangeMouseImage(mouse, toImage);
            yield return new WaitForSeconds(delay / 2f);
            ChangeMouseImage(mouse, empty);
            yield return AnimateMouseImage(mouse, toImage);
        }
    }

    void ChangeMouseImage(Image mouse, Sprite to)
    {
        mouse.sprite = to;
    }
    void DisableAllUI()
    {
        HashSet<Image> allImageElements = new HashSet<Image>
        {
            Mouse1,
            Mouse2,
            Space,
            MiniMapImage,
            HideInfoImage,
        };
        HashSet<TextMeshProUGUI> allTextElements = new HashSet<TextMeshProUGUI>
        {
            WasdInst,
            Click1Inst,
            Click2Inst,
            SpaceInst,
            MiniMapInst,
            HideInfoInst,
        };

        foreach (Image image in WASD)
            image.color = image.color.WithAlpha(0);

        foreach (Image image in allImageElements)
            image.color = image.color.WithAlpha(0);

        foreach (TextMeshProUGUI text in allTextElements)
            text.color = text.color.WithAlpha(0);



    }

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        DisableAllUI();
        Cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
