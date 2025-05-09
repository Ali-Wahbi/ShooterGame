using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Image panelImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private RectTransform resume, options, quit;
    [SerializeField] PausePowerDisplayer pausePowerDisplayer;
    [SerializeField] PauseWeaponDisplayer pauseWeaponDisplayer;

    bool isPaused = false;

    private static PauseScreen _g;

    public static PauseScreen p
    {
        get
        {
            if (_g == null)
            {
                _g = Instantiate(Resources.Load<PauseScreen>("PauseScreen"));
            }
            return _g;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //panel animation
    float panelAlphaMax = 0.95f;
    float panelAlphaStart = 0.0f;
    float panelAlpha
    {
        set
        {
            Color color = panelImage.color;
            color.a = value;
            panelImage.color = color;
        }
    }
    float buttonsDelay = 0.2f;
    float ButtonsXStart = -1400f;
    float ButtonsXEnd = -680f;
    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;

        SetPauseStart();

        pausePowerDisplayer.ShowPowers();
        pauseWeaponDisplayer.ShowWeapons();

        Cursor.visible = true;
        Time.timeScale = 0f;

        DOTween.To(() => panelAlphaStart, x => panelAlpha = x, panelAlphaMax, 0.5f)
        .SetUpdate(UpdateType.Normal, true)
        .OnComplete(() =>
        {
            // move the three buttons to position x -680
            DOTween.To(() => ButtonsXStart, x => resume.anchoredPosition = new Vector2(x, resume.anchoredPosition.y), ButtonsXEnd, 0.5f).SetUpdate(UpdateType.Normal, true).SetDelay(buttonsDelay * 0);
            DOTween.To(() => ButtonsXStart, x => options.anchoredPosition = new Vector2(x, options.anchoredPosition.y), ButtonsXEnd, 0.5f).SetUpdate(UpdateType.Normal, true).SetDelay(buttonsDelay * 1);
            DOTween.To(() => ButtonsXStart, x => quit.anchoredPosition = new Vector2(x, quit.anchoredPosition.y), ButtonsXEnd, 0.5f).SetUpdate(UpdateType.Normal, true).SetDelay(buttonsDelay * 2);
        });


    }

    void SetPauseStart()
    {
        canvasGroup.alpha = 1f;
        panelAlpha = 0f;

        resume.anchoredPosition = new Vector2(ButtonsXStart, resume.anchoredPosition.y);
        options.anchoredPosition = new Vector2(ButtonsXStart, options.anchoredPosition.y);
        quit.anchoredPosition = new Vector2(ButtonsXStart, quit.anchoredPosition.y);
    }

    public void OnResumeButtonClicked()
    {
        // resume the game and hide the pause screen
        Time.timeScale = 1f;
        // animator.SetTrigger("Hide");
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, 0.35f);
        // hide the power list when the game is resumed

        Cursor.visible = false;
        isPaused = false;
    }

    public void OnQuitButtonClicked()
    {
        Time.timeScale = 1f;
        Debug.Log("Quit Clicked");
        SceneChangeManager.Scm.QuitToMainScreen();
        // Show Quit Menu - WIP 
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Clicked");
        // Show Settings Menu - WIP
    }
}
