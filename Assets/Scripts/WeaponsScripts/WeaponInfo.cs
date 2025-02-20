using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;

public class WeaponInfo : MonoBehaviour
{
    [SerializeField] SpriteRenderer Icon, Frame;

    [Header("Texts: ")]
    [SerializeField] TextMeshPro WeaponName;
    [SerializeField] TextMeshPro Type, Damage, Ammo, Speed, Description;
    string nameSuffix = "Name: ";
    string DamageSuffix = "Damage: ";
    string AmmoSuffix = "Ammo: ";
    string SpeedSuffix = "Speed: ";

    // Start is called before the first frame update
    public void ShowInfo(AttackingWeapon weapon)
    {
        Icon.sprite = weapon.GetWeaponSprite();
        // WeaponName.text = nameSuffix + weapon.GetWeaponName();
        SetTextMesh(WeaponName, weapon.GetWeaponName(), nameSuffix);

        SetTypeText(weapon.GetWeaponType());

        Damage.text = DamageSuffix + weapon.GetWeaponDamage();
        Ammo.text = AmmoSuffix + weapon.GetWeaponAmmo();
        Speed.text = SpeedSuffix + weapon.GetWeaponReloadSpeed();

        SetTextMesh(Description, weapon.GetWeaponDescription());

    }

    void SetTypeText(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Melee:
                Type.text = "Melee";
                break;
            default:
                Type.text = "Ranged";
                break;
        }
    }

    void SetTextMesh(TextMeshPro holder, string text, string Suffix = "")
    {
        holder.text = Suffix + text;
    }

    public void HideInfo()
    {
        Destroy(gameObject);
    }
}
