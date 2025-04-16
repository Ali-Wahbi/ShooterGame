using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowersSingleton : MonoBehaviour
{
    private static WeaponPowersSingleton _instance { get; set; }

    public static WeaponPowersSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<WeaponPowersSingleton>("WeaponPowersSingleton"));
            }
            return _instance;
        }
    }

    public List<WeaponPowersType> weaponPowers = new List<WeaponPowersType>();

    public void UsePower(WeaponPowersType powerType)
    {
        weaponPowers.Add(powerType);

        switch (powerType)
        {
            case WeaponPowersType.SixFingers:
                // Implement logic for SixFingers power
                Debug.Log("SixFingers power used");
                break;
            case WeaponPowersType.Resoncane:
                // Implement logic for Resoncanec power
                Debug.Log("Resoncanee power used");
                break;
            default:
                Debug.LogWarning("Unknown weapon power type: " + powerType);
                break;
        }
    }

    private void OnEnable()
    {
        Debug.Log("WeaponPowersSingleton enabled");
    }

    private void OnDisable()
    {
        Debug.Log("WeaponPowersSingleton disabled");
    }

}
