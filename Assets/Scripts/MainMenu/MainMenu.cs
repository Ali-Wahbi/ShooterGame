using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private InfoDisplayer infoDisplayer;
    [SerializeField] private CanvasGroup infoButton;
    [SerializeField] private RectTransform DemoInst;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private int NumberOfAnimations = 1;

    private string currentAnimation = "Enter";

    int PickRandomAnim() => Random.Range(1, NumberOfAnimations);
    // Start is called before the first frame update
    void Start()
    {
        playPanel.SetActive(false);
        TweenInfoButton(delay: 3.3f);
        currentAnimation += PickRandomAnim().ToString();
        animator.SetTrigger(currentAnimation);
        Cursor.visible = true;

        float originalPos = DemoInst.anchoredPosition.x;

        DemoInst.DOAnchorPosX(-230f, 0f);
        DemoInst.DOAnchorPosX(originalPos, 1f).SetEase(Ease.OutCubic).SetDelay(3f);
    }

    void TweenInfoButton(float fromValue = 0f, float toValue = 1f, float delay = 0f)
    {
        infoButton.alpha = fromValue;
        infoButton.DOFade(toValue, 0.35f).SetDelay(delay)
        .OnComplete(() => infoButtonShown = !infoButtonShown);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInfoButtonClicked();
        }
    }
    void LoadEasyRoom()
    {
        // Load the game scene
        Debug.Log("Loading Easy Room");
        SceneChangeManager.Scm.LoadLevel(1);
    }
    void LoadMediumRoom()
    {
        // Load the game scene
        Debug.Log("Loading Medium Room");
        SceneChangeManager.Scm.LoadLevel(2);
    }

    public bool infoButtonShown = false;

    public void OnInfoButtonClicked()
    {
        if (!infoButtonShown) return;
        // if (infoButton.alpha == 1) TweenInfoButton(fromValue: 1, toValue: 0);
        infoDisplayer.ToggleInfoDisplayer();
        // if (infoButton.alpha == 0) TweenInfoButton(fromValue: 0, toValue: 1, delay: 2);
    }

    public void OnPlayButtonClicked()
    {
        // Load the game scene
        Debug.Log("Play Clicked");
        playPanel.SetActive(true);
        if (infoButtonShown)
            TweenInfoButton(fromValue: 1, toValue: 0);
        infoButtonShown = false;
    }
    public void OnEasyButtonClicked()
    {
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        LoadEasyRoom();
    }
    public void OnMediumButtonClicked()
    {
        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        LoadMediumRoom();
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit Clicked");
        // quit the game
        Application.Quit();
    }
    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Clicked");
        // Show Settings Menu - WIP
    }
}
