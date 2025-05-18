using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private int nextRoomIndex;
    [SerializeField] RectTransform RectUIELementsParent;
    [SerializeField] List<RectTransform> RectUIELements;
    private void Awake()
    {
        RectUIELementsParent.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // replace by the elevator animation
            // Load the next level
            // GoToNextLevel();
            ShowEndLevelMessage();
        }
    }

    void GoToNextLevel()
    {
        // Load the next level
        SceneChangeManager.Scm.LoadLevel(nextRoomIndex);
    }

    void ShowEndLevelMessage()
    {
        RectUIELementsParent.gameObject.SetActive(true);
        for (int i = 0; i < RectUIELements.Count; i++)
            TweenRect(rect: RectUIELements[i], Delay: i * 0.3f);

    }

    void TweenRect(RectTransform rect, float Delay = 0)
    {
        Vector3 originalValue = rect.localScale;
        rect.localScale = Vector3.zero;

        rect.DOScale(originalValue, 1f)
        .SetEase(Ease.InOutCubic).SetDelay(Delay);
    }

    // called by buttons UI
    public void OnReplayClicked() => SceneChangeManager.Scm.RetryFromStart();

    public void OnQuitClicked() => SceneChangeManager.Scm.QuitToMainScreen();

}
