using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : SlashWeapon
{
    public override void DynamicSlash(Transform startTran, Transform startRot)
    {
        // instantaite the slash here 
        GameObject _slash = Instantiate(slash, startTran.position, startRot.rotation);
        _slash.GetComponent<ScytheSlashEffect>().Setup(
            damage: (int)GetWeaponDamage(),
            rotation: startRot,
            flip: isFliped
            );
    }
}
