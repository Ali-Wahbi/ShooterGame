using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.UI;

/// <summary>
/// This class is responsible for controlling the progress bar in the loading screen.
/// It updates the progress bar value and the text displayed on it.
/// </summary>
public class ProgressBarController : MonoBehaviour
{
    /// <summary>
    /// The slider component of the progress bar.
    /// </summary>
    [Header("Progress Bar")]
    [SerializeField] private Slider slider;
    /// <summary>
    /// The fill image of the progress bar.
    /// </summary>
    [SerializeField] private Image fill;
    /// <summary>
    /// The text displayed <b>In Front Of</b> the progress bar.
    /// </summary>
    [SerializeField] private TextMeshProUGUI progressTextBlack;
    /// <summary>
    /// The text displayed <b>Behind</b> the progress bar.
    /// </summary>
    [SerializeField] private TextMeshProUGUI progressTextWhite;

    [Header("Colors")]
    /// <summary>
    /// the foreground color. used for the progress bar and the foreground text.
    /// </summary>
    [SerializeField] private Color foregroundColor;
    /// <summary>
    /// the background color. used for the background text.
    /// </summary>
    [SerializeField] private Color backgroundColor;

    private void Start()
    {
        SetProgreesPercentage(0);
        SetBlackTextPos(0);
        SetColors();

    }

    /// <summary>
    /// Set the colors of the progress bar and the texts.
    /// </summary>
    private void SetColors()
    {
        progressTextWhite.color = foregroundColor;
        fill.color = foregroundColor;

        progressTextBlack.color = backgroundColor;
    }

    /// <summary>
    /// Set the progress percentage text.
    /// </summary>
    /// <param name="percentage"></param>
    void SetProgreesPercentage(float percentage)
    {
        progressTextBlack.text = percentage.ToString("0") + "%";
        progressTextWhite.text = percentage.ToString("0") + "%";
    }

    float MaxPosX = 70.0f;
    /// <summary>
    /// Set the position of the black text based on the percentage of the progress bar.
    /// </summary>
    /// <param name="percentage">from the slider</param>
    void SetBlackTextPos(float percentage)
    {
        float newPosX = MaxPosX - MaxPosX * percentage / 100.0f;
        progressTextBlack.transform.localPosition = new Vector3(newPosX, 0, 0);
        // progressTextBlack.rectTransform.sizeDelta = new Vector2(originalWidth * percentage / 100.0f, progressTextBlack.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// called by the loading screen manager to set the progress bar value
    /// </summary> 
    public void SetSliderValue(float value)
    {
        // slider.value = Mathf.Lerp(slider.value, value, Time.deltaTime * 5.0f);
        var startValue = slider.value;

        DOTween.To(() => startValue, x => slider.value = x, value, 0.4f);

    }

    /// <summary>
    /// called by the slider when its value changes
    /// </summary>
    /// <param name="value"></param>
    public void OnSliderValueChanged(Single value)
    {
        SetProgreesPercentage(value);
        SetBlackTextPos(value);
    }
}
