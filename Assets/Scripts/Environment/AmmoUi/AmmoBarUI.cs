using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
        Holder.fillAmount = ammoPercentage;
        AmmoTextForeground.text = ((int)value).ToString("0");
        AmmoTextBackground.text = ((int)value).ToString("0");
    }


}
