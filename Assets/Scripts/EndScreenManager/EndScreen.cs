using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

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

    public static EndScreen Create()
    {
        Transform endScreen = Instantiate(GameAssets.g.EndScreenPrefap);
        EndScreen EndScreenPrefap = endScreen.GetComponent<EndScreen>();
        Debug.Log("Creating EndScreen.");

        return EndScreenPrefap;

    }

    #endregion


    #region Animations
    public void PlayInAnim()
    {
        if (animator == null) animator = GetComponent<Animator>();
        // Start the animation
        animator.SetTrigger("PlayIn");
    }
    public void PlayOutAnim()
    {
        // Start the animation
        animator.SetTrigger("PlayOut");
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
