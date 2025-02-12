using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AmmunitionUI : MonoBehaviour
{
    // Bulltes: Replace the string with an icon
    [SerializeField] private TextMeshProUGUI BullesText;
    string BulletPrefix = "Bullets: ";
    public Color originBulletColor;

    // Arrows: Replace the string with an icon
    [SerializeField] private TextMeshProUGUI ArrowsText;
    string ArrowPrefix = "Arrows: ";
    public Color originArrowColor;

    [SerializeField] Color NoAmmoColor = Color.red;

    private void Start()
    {
        // Set the original color of the texts UI
        SetTextColor(BullesText, originBulletColor);
        SetTextColor(ArrowsText, originArrowColor);

        // SetAmmo(WeaponType.Bullets, bult);
        // SetAmmo(WeaponType.Arrows, arrow);


    }
    public void SetAmmo(WeaponType weaponType, int newAmmo)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:
                SetBullets(newAmmo);
                break;
            case WeaponType.Arrows:
                SetArrows(newAmmo);
                break;
            default:
                break;
        }
    }

    private void SetBullets(int newAmmo)
    {
        BullesText.text = BulletPrefix + FormatNumberWithLeadingZeroes(newAmmo);
        if (newAmmo == 0)
            SetTextColor(BullesText, NoAmmoColor);
        else
            SetTextColor(BullesText, originBulletColor);
    }

    private void SetArrows(int newAmmo)
    {
        ArrowsText.text = ArrowPrefix + FormatNumberWithLeadingZeroes(newAmmo);
        if (newAmmo == 0)
            SetTextColor(ArrowsText, NoAmmoColor);
        else
            SetTextColor(ArrowsText, originArrowColor);
    }

    private string FormatNumberWithLeadingZeroes(int number, int totalLength = 3)
    {
        return number.ToString("D" + totalLength);
    }

    void SetTextColor(TextMeshProUGUI textMesh, Color color)
    {
        textMesh.color = color;
    }
}
