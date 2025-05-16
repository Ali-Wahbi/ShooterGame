using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterGun : ShootingWeapon
{
    override public void DynamicFire(Transform startTran, Transform startRot, Vector2 CursorPos = default)
    {
        GameObject bult = Instantiate(bullet, startTran.position, startRot.rotation);
        bult.GetComponent<TeleporterBullet>().SetBulletSpeedAndDamage(bulletSpeed, 0f);



    }
}
