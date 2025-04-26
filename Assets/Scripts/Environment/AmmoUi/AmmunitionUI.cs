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


}
