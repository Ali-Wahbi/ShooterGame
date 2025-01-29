using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : SlashWeapon
{
    public override void DynamicSlash(Transform startTran, Transform startRot)
    {
        // instantaite the slash here 
        Instantiate(slash, startTran.position, startRot.rotation);

    }

}
