using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSword : SlashWeapon
{
    [SerializeField] public GameObject glassBullet;

    public override void DynamicSlash(Transform startTran, Transform startRot)
    {
        // instantaite the slash here 
        GameObject _slash = Instantiate(slash, startTran.position, startRot.rotation);
        _slash.GetComponent<SlashEffect>().Setup(damage: (int)GetWeaponDamage());
        _slash.AddComponent<GlassSwordSlash>().SetParent(this);
    }

    public void OnSlashHitBullet(Vector3 pos, Quaternion rot)
    {
        Instantiate(glassBullet, pos, rot);
    }
}
