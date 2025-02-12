using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nug : ShootingWeapon
{
    public override void DynamicFire(Transform startTran, Transform startRot, Vector2 CursorPos = default)
    {
        // Reverse the rotation by adding 180 degrees
        Quaternion reversedRotation = startRot.rotation * Quaternion.Euler(0, 180, 0);

        // shoot the bullet
        GameObject bult = Instantiate(bullet, CursorPos, reversedRotation);
        bult.GetComponent<RevBullet>().SetBulletSpeedDamagePos(bulletSpeed, GetWeaponDamage(), startTran.position);

    }


}
