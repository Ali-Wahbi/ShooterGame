using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class AmmoBarUI : MonoBehaviour
{
    [SerializeField] private float MaxAmmo = 100f;
    [SerializeField] private float MinAmmo = 0f;

    [SerializeField] private int CurrentAmmo = 100;

    [Header("UI Elements")]
    [SerializeField] Image Holder;
    [SerializeField] TextMeshProUGUI AmmoTextForeground;
    [SerializeField] TextMeshProUGUI AmmoTextBackground;


    public void SetupBar(float maxAmmo, float minAmmo)
    {
        MaxAmmo = maxAmmo;
        MinAmmo = minAmmo;
        UpdateBar(CurrentAmmo);
    }

    public void SetAmmo(int newAmmo)
    {
        CurrentAmmo = Mathf.Clamp(newAmmo, (int)MinAmmo, (int)MaxAmmo);
        UpdateBar(CurrentAmmo);
    }


    public void SetNumberTextActive(bool isActive)
    {
        AmmoTextForeground.gameObject.SetActive(isActive);
        AmmoTextBackground.gameObject.SetActive(isActive);
    }

    void UpdateBar(float value)
    {
        float ammoPercentage = (value - MinAmmo) / (MaxAmmo - MinAmmo);
        // Debug.Log("Ammo Percentage: " + ammoPercentage);
        SetFillAmount(ammoPercentage);
        AmmoTextForeground.text = ((int)value).ToString("0");
        AmmoTextBackground.text = ((int)value).ToString("0");
    }
    DOTween tween;
    void SetFillAmount(float value)
    {
        if (tween != null)
        {
            // tween.;
        }
        DOTween.To(() => Holder.fillAmount, x => Holder.fillAmount = x, value, 0.5f).SetEase(Ease.OutSine);
        // Holder.fillAmount = value;
    }

}
