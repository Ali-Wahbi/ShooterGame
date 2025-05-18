using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;

using Exentsions;
public class WeaponInfo : MonoBehaviour
{
    [SerializeField] SpriteRenderer Frame;
    [SerializeField] Transform IconTransform;

    [Header("Texts: ")]
    [SerializeField] TextMeshPro WeaponName;
    [SerializeField] TextMeshPro Type, Damage, Ammo, Speed, Description;
    string nameSuffix = "";
    string typeSuffix = "Type: ";
    string DamageSuffix = "Damage: ";
    string AmmoSuffix = "Ammo: ";
    string SpeedSuffix = "Speed: ";

    public Vector3 OriginalPos, OriginalScale;
    public Vector3 RangedPos, RangedScale;
    private void Awake()
    {

        OriginalPos = IconTransform.position.Add();
        OriginalScale = IconTransform.localScale;
        RangedPos = IconTransform.position.Add(y: 0.5f);
        RangedScale = new Vector3(10, 10, 10);
    }
    public void SetInfo(AttackingWeapon weapon)
    {
        SetSprite(weapon.GetWeaponSprite());
        // WeaponName.text = nameSuffix + weapon.GetWeaponName();
        SetTextMesh(WeaponName, weapon.GetWeaponName(), nameSuffix);

        SetTypeText(weapon.GetWeaponType());
        SetIconTransform(weapon.GetWeaponType());

        Damage.text = DamageSuffix + weapon.GetWeaponDamage();
        Ammo.text = AmmoSuffix + weapon.GetWeaponAmmo();
        Speed.text = SpeedSuffix + weapon.GetWeaponReloadSpeed();

        SetTextMesh(Description, weapon.GetWeaponDescription());

    }

    void SetSprite(Sprite sprite)
    {
        IconTransform.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void SetTypeText(WeaponType type)
    {
        string typeText = "";
        switch (type)
        {
            case WeaponType.Melee:
                typeText = "Melee";
                break;
            default:
                typeText = "Ranged";
                break;
        }
        SetTextMesh(Type, typeText, typeSuffix);
    }

    void SetTextMesh(TextMeshPro holder, string text, string Suffix = "")
    {
        holder.text = Suffix + text;
    }
    void SetIconTransform(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Melee:
                SetTrasform(OriginalPos, OriginalScale);
                break;
            default:
                SetTrasform(RangedPos, RangedScale);
                break;
        }

        void SetTrasform(Vector3 pos, Vector3 scale)
        {
            IconTransform.position = pos;
            IconTransform.localScale = scale;
        }
    }

    public void HideInfo()
    {
        Destroy(gameObject);
    }
}
