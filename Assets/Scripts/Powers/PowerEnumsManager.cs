using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Power
{
    public string PowerName;
    public Sprite PowerIcon;
    public Sprite PowerOutlinedIcon;

    [TextArea(3, 7)]
    public string PowerDescription;
}


public enum WeaponPowersType
{
    None, SixFingers, Resonance, GreedyAmmo, PositiveFeedback
}

public enum PlayerPowersType
{
    None, SASExtension, ExtraMagazine, Binoculars, EnhancedSprinters, RechargableBatteries
}
