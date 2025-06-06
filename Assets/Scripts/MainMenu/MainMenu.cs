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
    [SerializeField] private int NumberOfAnimations = 1;

    private string currentAnimation = "Enter";

    int PickRandomAnim() => Random.Range(1, NumberOfAnimations);
    // Start is called before the first frame update
    void Start()
    {
        TweenInfoButton(delay: 5f);
        currentAnimation += PickRandomAnim().ToString();
        animator.SetTrigger(currentAnimation);
        Cursor.visible = true;
    }

    void TweenInfoButton(float fromValue = 0f, float toValue = 1f, float delay = 0f)
    {
        infoButton.alpha = fromValue;
        infoButton.DOFade(toValue, 0.35f).SetDelay(delay);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnInfoButtonClicked();
        }
    }
    void LoadFirstRoom()
    {
        // Load the game scene
        Debug.Log("Loading First Room");
        SceneChangeManager.Scm.LoadLevel(1);
    }

    public void OnInfoButtonClicked()
    {
        if (infoButton.alpha == 1) TweenInfoButton(fromValue: 1, toValue: 0);
        infoDisplayer.ToggleInfoDisplayer();
        if (infoButton.alpha == 0) TweenInfoButton(fromValue: 0, toValue: 1, delay: 2);
    }

    public void OnPlayButtonClicked()
    {
        // Load the game scene
        Debug.Log("Play Clicked");

        GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        LoadFirstRoom();
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
