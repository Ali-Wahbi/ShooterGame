using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiNumGun : ShootingWeapon
{
    int GetChance => Random.Range(0, 100);
    bool ShootZero = false;
    override public void DynamicFire(Transform startTran, Transform startRot, Vector2 CursorPos = default)
    {
        ShootZero = GetChance < 40;

        GameObject bult = Instantiate(bullet, startTran.position, startRot.rotation);
        bult.GetComponent<BinumBullet>().SetBulletSpeedAndDamage(bulletSpeed, ShootZero ? 0 : 1);



    }
}
