using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class AttackingWeapon : MonoBehaviour
{
    public Weapon weapon;
    public bool canFire = true;

    // abstract public void Attack(Vector3 startPos, Quaternion startRot);
    abstract public void DynamicAttack(Transform startTran, Transform startRot, Transform sprite);

    public float GetWeaponDamage() => weapon.WeaponDamage;


    public WeaponType GetWeaponType() => weapon.WeaponType;

    public WeaponAnim GetWeaponAnim() => weapon.WeaponAnim;

    public Sprite GetWeaponSprite() => weapon.WeaponSprite;

    public float GetWeaponReloadSpeed() => weapon.WeaponSpeed;
}
