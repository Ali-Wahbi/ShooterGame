using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : ShootingWeapon
{
    public override void DynamicFire(Transform startTran, Transform startRot, Vector2 CursorPos = default)
    {

        GameObject bult = Instantiate(bullet, startTran.position, startRot.rotation);
        bult.GetComponent<Arrow>().SetBulletSpeedAndDamage(bulletSpeed, GetWeaponDamage());
    }
}
