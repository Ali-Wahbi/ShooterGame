using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleText : MonoBehaviour
{
    [SerializeField] private Animator animationController;
    [SerializeField] private TextMeshProUGUI textMesh;
    [SerializeField] private bool PlayAtSart = true;

    // called when the object is created
    public void SetUpText(string text, float duration = 2.5f)
    {
        // Set the text
        textMesh.text = text;

        if (!PlayAtSart) return;

        PlayInAnim();

        StartCoroutine(WaitForOut(duration));
    }

    IEnumerator WaitForOut(float duration)
    {
        yield return new WaitForSeconds(duration);
        // Start the animation
        animationController.SetTrigger("PlayOut");
    }

    public void PlayInAnim()
    {
        // Start the animation
        animationController.SetTrigger("PlayIn");
    }

    // Called at the end of the out animation by the animator
    public void OnOutAnimEnds()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}
