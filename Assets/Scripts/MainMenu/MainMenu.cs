using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private int NumberOfAnimations = 1;
    [SerializeField] private ProgressBarController progressBarController;

    private string currentAnimation = "Enter";

    int PickRandomAnim() => Random.Range(1, NumberOfAnimations);
    // Start is called before the first frame update
    void Start()
    {
        currentAnimation += PickRandomAnim().ToString();
        animator.SetTrigger(currentAnimation);
        Cursor.visible = true;
    }

    void LoadFirstRoom()
    {
        // Load the game scene
        Debug.Log("Loading First Room");
        SceneChangeManager.Scm.LoadLevel(1);
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
