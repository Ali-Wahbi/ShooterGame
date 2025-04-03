using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleText : MonoBehaviour
{
    [SerializeField] private Animator animationController;
    [SerializeField] private TextMeshProUGUI textMesh;


    // called when the object is created
    public void SetUpText(string text, float duration = 2.5f)
    {
        // Set the text
        textMesh.text = text;


        // Start the animation
        animationController.SetTrigger("PlayIn");

        StartCoroutine(WaitForOut(duration));
    }

    IEnumerator WaitForOut(float duration)
    {
        yield return new WaitForSeconds(duration);
        // Start the animation
        animationController.SetTrigger("PlayOut");
    }

    public void OnOutAnimEnds()
    {
        // Destroy the game object
        Destroy(gameObject);
    }
}
