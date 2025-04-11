using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    void Start()
    {
        Healthbar.AddFirstFilled();
        // Set the UI start stats
        Healthbar.IncMaxShield(MaxSheild);
        Healthbar.DecFromShield(MissingShield);

        CurrentShield = MaxSheild - MissingShield;

        // set the shield start stat
        shieldIsBroken = CurrentShield == 0;
        shieldIsFull = CurrentShield == MaxSheild;


        AmmoUIController.SetAmmo(WeaponType.Bullets, BulletsAmmo);
        AmmoUIController.SetAmmo(WeaponType.Arrows, ArrowsAmmo);
    }

    #region SAS
    [SerializeField] private HealthbarController Healthbar;

    // remove public later
    public int MaxSheild;

    public int CurrentShield;

    public int MissingShield = 0;


    // remove public later
    bool shieldIsBroken = false;
    bool shieldIsFull = false;

    //Testinig only
    bool canDie = false;



    public void TakeDamage(int damage)
    {
        // Player dies if hit while shield is broken
        if (shieldIsBroken && canDie)
        {
            Debug.Log("Player Died");
            Healthbar.DecFromShield(1);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            ShowEndScreen();
            return;
        }

        if (shieldIsFull) shieldIsFull = false;

        // Take Damage unitll 0 
        if (CurrentShield - damage <= 0)
        {
            // Decrease all the shield, no extra damage
            Healthbar.DecFromShield(CurrentShield);

            CurrentShield = 0;
            MissingShield = MaxSheild;

            shieldIsBroken = true;
            return;
        }

        // Normal decrease of the shield
        CurrentShield -= damage;
        SetMissingShield();
        Healthbar.DecFromShield(damage);
    }


    public void RechargeShield(int recharge)
    {
        // fill shield unitll max shield
        if (shieldIsFull) return;
        // fix broken shield
        if (shieldIsBroken) shieldIsBroken = false;

        // handle over charge
        if (CurrentShield + recharge >= MaxSheild)
        {
            Healthbar.IncShield(MaxSheild - CurrentShield);

            shieldIsFull = true;
            CurrentShield = MaxSheild;

            SetMissingShield();

            return;
        }

        // Normal recharge
        CurrentShield += recharge;
        SetMissingShield();
        Healthbar.IncShield(recharge);
    }

    // set the missing shield
    void SetMissingShield()
    {
        MissingShield = MaxSheild - CurrentShield;
    }

    public int GetMissingShield()
    {
        return MissingShield;
    }

    public void IncMaxShield(int addition)
    {
        MaxSheild += addition;
        // Handle UI
    }

    public void DecMaxShield(int subtraction)
    {
        MaxSheild -= subtraction;
        // Handle UI
    }

    void ShowEndScreen()
    {
        FindAnyObjectByType<EndScreen>()?.PlayInAnim();
    }

    #endregion





    #region AmmoUI
    // defaults to 0
    public int BulletsAmmo;
    public int ArrowsAmmo;
    int maxBullets = 999;
    int maxArrows = 999;

    [SerializeField] private AmmunitionUI AmmoUIController;

    private void SetAmmo(WeaponType weaponType, int newAmmo)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:

                BulletsAmmo += newAmmo;
                BulletsAmmo = Mathf.Clamp(BulletsAmmo, 0, maxBullets);

                AmmoUIController.SetAmmo(WeaponType.Bullets, BulletsAmmo);
                break;
            case WeaponType.Arrows:

                ArrowsAmmo += newAmmo;
                ArrowsAmmo = Mathf.Clamp(ArrowsAmmo, 0, maxArrows);

                AmmoUIController.SetAmmo(WeaponType.Arrows, ArrowsAmmo);
                break;
            default:
                break;
        }
    }


    public void AddAmmo(WeaponType weaponType, int ammo)
    {
        SetAmmo(weaponType, ammo);
    }

    public void DecAmmo(WeaponType weaponType, int ammo)
    {
        SetAmmo(weaponType, -ammo);
    }


    public int GetAmmo(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:
                return BulletsAmmo;
            case WeaponType.Arrows:
                return ArrowsAmmo;
            case WeaponType.Melee:
                return 1;
            default:
                return 0;
        }
    }

    #endregion


    [ContextMenu("Inc30Bullets")]
    void RechargeBullets()
    {
        AddAmmo(WeaponType.Bullets, 30);
    }

    [ContextMenu("Inc30Arrows")]
    void RechargeArrows()
    {
        AddAmmo(WeaponType.Arrows, 30);
    }
}
