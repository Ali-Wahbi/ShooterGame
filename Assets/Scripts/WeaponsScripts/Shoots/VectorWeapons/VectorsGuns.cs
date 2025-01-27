using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorsGuns : ShootingWeapon
{

    public int bulletCount = 1;
    public override void Fire(Vector3 startPos, Quaternion startRot)
    {
        StartCoroutine(Shoot3Bullets(startPos, startRot));
    }

    public override void DynamicFire(Transform startTran, Transform startRot)
    {
        StartCoroutine(Shoot3DynamicFire(startTran, startRot));
    }

    IEnumerator Shoot3Bullets(Vector3 startPos, Quaternion startRot)
    {

        for (int i = 0; i < bulletCount; i++)
        {
            // shoot the bullet
            GameObject bult = Instantiate(bullet, startPos, startRot);
            bult.GetComponent<VectorBullet>().SetBulletSpeedAndDamage(bulletSpeed, GetWeaponDamage());
            yield return new WaitForSeconds(0.1f);

        }
    }
    IEnumerator Shoot3DynamicFire(Transform startTran, Transform startRot)
    {

        for (int i = 0; i < bulletCount; i++)
        {
            // shoot the bullet
            GameObject bult = Instantiate(bullet, startTran.position, startRot.rotation);
            bult.GetComponent<VectorBullet>().SetBulletSpeedAndDamage(bulletSpeed, GetWeaponDamage());
            yield return new WaitForSeconds(0.1f);

        }
    }
}
