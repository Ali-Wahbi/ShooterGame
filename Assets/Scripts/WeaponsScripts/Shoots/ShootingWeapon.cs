using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ShootingWeapon : AttackingWeapon
{
    public GameObject bullet;
    public float bulletSpeed;
    public override void Attack(Vector3 startPos, Quaternion startRot)
    {
        canFire = false;
        Fire(startPos, startRot);
        Timer();
    }

    public override void DynamicAttack(Transform startTran, Transform startRot)
    {
        canFire = false;
        DynamicFire(startTran, startRot);
        Timer();
    }

    abstract public void Fire(Vector3 startPos, Quaternion startRot);
    abstract public void DynamicFire(Transform startTran, Transform startRot);
    void Timer()
    {
        StartCoroutine(StartTimer());
    }

    // Fire rate timer
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(GetWeaponReloadSpeed());
        Debug.Log("Timer ended");
        canFire = true;

    }
}
