using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _g;

    public static GameAssets g
    {
        get
        {
            if (_g == null)
            {
                _g = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            }
            return _g;
        }
    }

    [Tooltip("used to show popup damage indicator.")]
    public Transform PopupPrefap;

    [Tooltip("gives the player ammo when picked up, used by Enemies and Destroyable objects.")]
    public Transform AmmoPickUpPrefap;

    [Tooltip("pick up weapon prefap.")]
    public Transform pickUpWeaponPrefap;

    [Tooltip("pick up weapon info box.")]
    public Transform WeaponInfo;

    [Tooltip("End Screen Prefap appears when the player dies.")]
    public Transform EndScreenPrefap;
}
