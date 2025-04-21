using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        animator.SetTrigger("Show");
        // show the power list when the game is paused
    }

    public void OnResumeButtonClicked()
    {
        // resume the game and hide the pause screen
        Time.timeScale = 1f;
        animator.SetTrigger("Hide");
        // hide the power list when the game is resumed

        Cursor.visible = false;
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit Clicked");
        // quit the game
        // Show Quit Menu - WIP 
    }

    public void OnSettingsButtonClicked()
    {
        Debug.Log("Settings Clicked");
        // Show Settings Menu - WIP
    }
}
