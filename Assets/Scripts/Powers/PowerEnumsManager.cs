using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Power
{
    public string PowerName;
    public Sprite PowerIcon;

    [TextArea(3, 7)]
    public string PowerDescription;
}


public enum WeaponPowersType
{
    None, SixFingers, Resoncane
}

public enum PlayerPowersType
{
    None, ExtraSAS, ExtraMagazine
}
