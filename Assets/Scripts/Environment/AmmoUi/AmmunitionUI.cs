using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AmmunitionUI : MonoBehaviour
{
    [SerializeField] AmmoBarUI BulletBarUI;
    [SerializeField] AmmoBarUI ArrowBarUI;

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


}
