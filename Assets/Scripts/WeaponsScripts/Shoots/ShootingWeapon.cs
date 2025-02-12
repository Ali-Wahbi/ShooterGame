using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

abstract public class ShootingWeapon : AttackingWeapon
{
    public GameObject bullet;
    public float bulletSpeed;


    public override void DynamicAttack(Transform startTran, Transform startRot, Transform sprite, Vector2 CursorPos = new Vector2())
    {
        canFire = false;
        DynamicFire(startTran, startRot, CursorPos);
        AnimataGun(sprite);
        Timer();
    }

    abstract public void DynamicFire(Transform startTran, Transform startRot, Vector2 CursorPos = new Vector2());
    void Timer()
    {
        StartCoroutine(StartTimer());
    }

    // Fire rate timer
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(GetWeaponReloadSpeed());
        // Debug.Log("Timer ended");
        canFire = true;

    }

    void AnimataGun(Transform gun)
    {
        StartCoroutine(AnimateShooting(gun));
        // Debug.Log("Animating: " + gun);
    }

    IEnumerator AnimateShooting(Transform gun)
    {
        gun.position = new Vector3(gun.position.x - 0.2f, gun.position.y, gun.position.z);
        yield return new WaitForSeconds(0.2f);
        gun.position = new Vector3(gun.position.x + 0.2f, gun.position.y, gun.position.z);
    }
}
