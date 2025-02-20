using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class AttackingWeapon : MonoBehaviour
{
    public Weapon weapon;
    public bool canFire = true;

    // abstract public void Attack(Vector3 startPos, Quaternion startRot);
    abstract public void DynamicAttack(Transform startTran, Transform startRot, Transform sprite, Vector2 CursorPos = new Vector2());

    public float GetWeaponDamage() => weapon.WeaponDamage;

    public string GetWeaponName() => weapon.WeaponName;
    public string GetWeaponDescription() => weapon.WeaponDiscription;

    public WeaponType GetWeaponType() => weapon.WeaponType;

    public WeaponAnim GetWeaponAnim() => weapon.WeaponAnim;

    public WeaponClasses GetWeaponClass() => weapon.WeaponClass;
    public int GetWeaponAmmo() => weapon.WeaponAmmo;

    public Sprite GetWeaponSprite() => weapon.WeaponSprite;

    public float GetWeaponReloadSpeed() => weapon.WeaponSpeed;

    public bool GetWeaponCanFire() => canFire;


}
