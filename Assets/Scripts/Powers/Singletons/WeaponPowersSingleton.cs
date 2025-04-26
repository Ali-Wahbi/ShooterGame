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
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneChangeManager.Scm.AddToRetryEvents(ResetPowers);
        SceneChangeManager.Scm.AddToQuitEvents(DestroySingleton);
    }
    public List<WeaponPowersType> weaponPowers = new List<WeaponPowersType>();

    [Header("Weapon Powers")]

    /// <summary>
    /// <b>Six fingers power:</b> increases the speed of the slash effect
    /// </summary>
    public float SlashSpeedMultiplierPower = 1f;

    /// <summary>
    /// <b>Resonance power:</b> allows the player to reflect bullets
    /// </summary>
    public bool CanReflect = false;


    /// <summary>
    /// <b>Greedy ammo power:</b> increases the amount of ammo picked up up to 50%
    /// </summary>
    public float AmmoPower = 1f;

    /// <summary>
    /// <b>Positive feedback power:</b> chance to returns ammo to the player when the weapon is reloaded
    /// </summary>
    public bool ReturnAmmo = false;
    //TODO: Apply the powers to the weapon

    public void UsePower(WeaponPowersType powerType)
    {
        weaponPowers.Add(powerType);

        switch (powerType)
        {
            case WeaponPowersType.SixFingers:
                SlashSpeedMultiplierPower = 3f;
                break;

            case WeaponPowersType.Resonance:
                CanReflect = true;
                break;

            case WeaponPowersType.GreedyAmmo:
                AmmoPower = 1.5f;
                break;

            case WeaponPowersType.PositiveFeedback:
                ReturnAmmo = true;
                break;

            default:
                Debug.LogWarning("Unknown weapon power type: " + powerType);
                break;
        }
    }

    void ResetPowers()
    {
        Debug.Log("Resetting weapon powers");
        weaponPowers.Clear();
        SlashSpeedMultiplierPower = 1f;
        CanReflect = false;
        AmmoPower = 1f;
        ReturnAmmo = false;
    }
    void DestroySingleton()
    {
        Debug.Log("Destroying weapon powers singleton");
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
            _instance = null;
        }
    }
}
