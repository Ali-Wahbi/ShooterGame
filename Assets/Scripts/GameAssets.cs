using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _g;

    public static GameAssets g
    {
        get
        {
            if (_g == null)
            {
                _g = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _g;
        }
    }

    [Tooltip("used to show popup damage indicator.")]
    public Transform PopupPrefap;

    [Tooltip("gives the player ammo when picked up, used by Enemies and Destroyable objects.")]
    public Transform AmmoPickUpPrefap;

    [Tooltip("pick up weapon prefap.")]
    public Transform pickUpWeaponPrefap;

    [Tooltip("pick up weapon info box.")]
    public Transform WeaponInfo;

    [Tooltip("End Screen Prefap appears when the player dies.")]
    public Transform EndScreenPrefap;

    [Tooltip("used to show the progress bar when loading a scene.")]
    public Transform ProgressBarPrefap;


    [Header("Format text colors")]
    [SerializeField] Color SASColor = Color.cyan;
    [SerializeField] Color AmmoColor;
    [SerializeField] Color MeleeColor;
    [SerializeField] Color RangedColor;

    public string FormatText(string textToFormat)
    {
        string SasColorHex = ColorUtility.ToHtmlStringRGBA(SASColor);
        string AmmoColorHex = ColorUtility.ToHtmlStringRGBA(AmmoColor);
        string MeleeColorHex = ColorUtility.ToHtmlStringRGBA(MeleeColor);
        string RangedColorHex = ColorUtility.ToHtmlStringRGBA(RangedColor);

        string formattedText = textToFormat.Replace("SAS", $"<color=#{SasColorHex}>SAS</color>")
                                            .Replace("Ammo", $"<color=#{AmmoColorHex}>Ammo</color>")
                                            .Replace("ammo", $"<color=#{AmmoColorHex}>ammo</color>")
                                            .Replace("Melee", $"<color=#{MeleeColorHex}>Melee</color>")
                                            .Replace("melee", $"<color=#{MeleeColorHex}>melee</color>")
                                            .Replace("ranged", $"<color=#{RangedColorHex}>ranged</color>")
                                            .Replace("Ranged", $"<color=#{RangedColorHex}>Ranged</color>");
        return formattedText;
    }
}
