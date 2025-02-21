using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string WeaponName;
    public Sprite WeaponSprite;
    public WeaponType WeaponType;
    public WeaponClasses WeaponClass;
    public WeaponAnim WeaponAnim;
    public int WeaponAmmo;
    public float WeaponDamage;
    public float WeaponSpeed;
    [TextArea(3, 7)]
    public string WeaponDiscription;

}
