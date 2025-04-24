using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    ProgressBarController progressBarController;
    [SerializeField] UnityEvent RetryEvents;
    [SerializeField] UnityEvent QuitEvents;

    private static SceneChangeManager _Sc;

    public static SceneChangeManager Scm
    {
        get
        {
            if (_Sc == null)
            {
                _Sc = Instantiate(Resources.Load<SceneChangeManager>("SceneChangeManager"));
            }
            return _Sc;
        }
    }

    bool canGo = false;
    AsyncOperation asyncLoad;
    public int delay = 1000;
    public int multi = 100;
    /// <summary>
    /// Load the level with the given index.
    /// </summary>
    /// <param name="levelIndex">must be a valid index from the build indexes</param>
    public async void LoadLevel(int levelIndex = 1)
    {
        if (progressBarController == null) MakeProgressBarController();
        // Load the game scene
        asyncLoad = SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            // Update the progress bar value and text
            progressBarController.SetSliderValue(asyncLoad.progress * multi);
            Debug.Log("Loading: " + asyncLoad.progress);
            if (asyncLoad.progress >= 0.9f)
            {
                progressBarController.SetSliderValue(100);
                asyncLoad.allowSceneActivation = true;
            }
            await Task.Delay(delay);
        }
    }

    void MakeProgressBarController()
    {
        progressBarController = Instantiate(GameAssets.g.ProgressBarPrefap).GetComponent<ProgressBarController>();
        progressBarController.gameObject.SetActive(true);
    }

    public void AddToRetryEvents(UnityAction action)
    {
        RetryEvents.AddListener(action);
    }
    public void AddToQuitEvents(UnityAction action)
    {
        QuitEvents.AddListener(action);
    }
    public void RetryFromStart()
    {
        // Retry the level
        Debug.Log("Retry from start level");
        RetryEvents.Invoke();
        // SceneManager.LoadSceneAsync("LapLevel-1");
        LoadLevel(1);
    }

    public void QuitToMainScreen()
    {
        // Quit to main Screen
        QuitEvents.Invoke();
        LoadLevel(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canGo)
        {
            canGo = false;
            asyncLoad.allowSceneActivation = true;
        }
    }
}
