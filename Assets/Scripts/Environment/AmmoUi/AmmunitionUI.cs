using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class AmmunitionUI : MonoBehaviour
{
    [SerializeField] AmmoBarUI BulletBarUI;
    [SerializeField] AmmoBarUI ArrowBarUI;
    [Header("Holder UI")]
    [SerializeField] RectTransform Holder;
    // [SerializeField] float YFirstPos = 270;
    [SerializeField] float YSecondPos = 140;
    [SerializeField] bool PutInSecondPos = false;

    float HolderYPos
    {
        set
        {
            SetHolderPos(value);
        }
        get
        {
            return Holder.anchoredPosition.y;
        }
    }

    private void Start()
    {
        if (PutInSecondPos) SetHolderPos(YSecondPos);

        // ChangeUiPos();
    }

    public void SetAmmo(WeaponType weaponType, int newAmmo)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:
                BulletBarUI.SetAmmo(newAmmo);
                break;
            case WeaponType.Arrows:
                ArrowBarUI.SetAmmo(newAmmo);
                break;
            default:
                break;
        }
    }
    public void SetMaxAmmo(WeaponType weaponType, int maxAmmo)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:
                BulletBarUI.SetupBar(maxAmmo, 0);
                break;
            case WeaponType.Arrows:
                ArrowBarUI.SetupBar(maxAmmo, 0);
                break;
            default:
                break;
        }
    }
    public void ChangeUiPos()
    {
        DOTween.To(() => HolderYPos, x => HolderYPos = x, YSecondPos, 1.0f).SetEase(Ease.OutSine);
        PutInSecondPos = true;
    }

    public void SetToSecondPos(bool toSecondPos)
    {
        PutInSecondPos = toSecondPos;
    }

    void SetHolderPos(float yPos)
    {
        Holder.anchoredPosition = Holder.anchoredPosition.With(y: yPos);
    }

}
