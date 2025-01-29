using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string WeaponName;
    public Sprite WeaponSprite;
    public WeaponType WeaponType;
    public WeaponAnim WeaponAnim;
    public float WeaponDamage;
    public float WeaponSpeed;

}
