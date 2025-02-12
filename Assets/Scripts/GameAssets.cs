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

    public Transform PopupPrefap;

    public Transform AmmoPickUpPrefap;

    public Transform pickUpWeaponPrefap;
}
