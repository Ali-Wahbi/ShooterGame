using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Image panelImage;
    [SerializeField] private RectTransform retryButton, quitButton, textHolder;


    #region Setup
    private static EndScreen _Es;

    public static EndScreen Es
    {
        get
        {
            if (_Es == null)
            {
                _Es = Instantiate(Resources.Load<EndScreen>("Prefabs/UI/EndScreen/EndScreen"));
            }
            return _Es;
        }
    }

    #endregion


    #region Animations
    // Panel animation
    float panelAlphaMax = 0.89f;
    float panelAlphaStart = 0.0f;
    float panelAlph
    {
        set
        {
            Color color = panelImage.color;
            color.a = value;
            panelImage.color = color;
        }
    }

    // Retry & Quit button animation
    float ButtonXEnd = -680f;
    float ButtonXStart = 0f;
    float RetryButtonX
    {
        set
        {
            Vector3 pos = retryButton.anchoredPosition;
            pos.x = value;
            retryButton.anchoredPosition = pos;
        }
    }
    float QuitButtonX
    {
        set
        {
            Vector3 pos = quitButton.anchoredPosition;
            pos.x = value;
            quitButton.anchoredPosition = pos;
        }
    }

    // Text animation
    float textYEnd = 0f;
    float textYStart = 0f;
    [SerializeField] Ease textEase = Ease.OutCubic;
    float textY
    {
        set
        {
            Vector3 pos = textHolder.anchoredPosition;
            pos.y = value;
            textHolder.anchoredPosition = pos;
            // Debug.Log("Text Y: " + pos.y);
        }
    }


    public void PlayInAnim()
    {
        float time = 1.0f;
        panelAlph = 0;
        DOTween.To(() => panelAlphaStart, x => panelAlph = x, panelAlphaMax, time).SetEase(Ease.OutCubic);

        ButtonXStart = -1200;

        DOTween.To(() => ButtonXStart, x => RetryButtonX = x, ButtonXEnd, time).SetEase(Ease.OutCubic);
        DOTween.To(() => ButtonXStart, x => QuitButtonX = x, ButtonXEnd, time).SetEase(Ease.OutCubic).SetDelay(0.1f);

        textYStart = textHolder.anchoredPosition.y;
        DOTween.To(() => textYStart, y => textY = y, textYEnd, time - 0.2f).SetEase(textEase).SetDelay(0.2f);

    }

    #endregion

    #region Clicks
    public void OnRetryClicked()
    {
        // Retry the level
        Debug.Log("Retry from start level");
        SceneChangeManager.Scm.RetryFromStart();
        // SceneManager.LoadSceneAsync("LapLevel-1");
        // SceneManager.LoadSceneAsync(0);
    }


    public void OnQuitClicked()
    {
        // Quit to main Screen
        SceneChangeManager.Scm.QuitToMainScreen();
        Debug.Log("Quit to main screen");
    }

    #endregion


    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N))
        // {
        //     PlayInAnim();
        // }
    }


}
