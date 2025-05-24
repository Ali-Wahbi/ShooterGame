using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    void Start()
    {

#if UNITY_EDITOR
        canDie = false;
#endif
        SetUiElements();
        Healthbar.AddFirstFilled();
        // Set the UI start stats
        SetMaxShield(MaxSheild);

        CurrentShield = MaxSheild - MissingShield;

        // set the shield start stat
        shieldIsBroken = CurrentShield == 0;
        shieldIsFull = CurrentShield == MaxSheild;


        AmmoUIController.SetAmmo(WeaponType.Bullets, BulletsAmmo);
        AmmoUIController.SetAmmo(WeaponType.Arrows, ArrowsAmmo);

        SetupMaxAmmo();
        // set the camera zoom
        SetCameraZoom();

        // save and load the player stats
    }

    void SetUiElements()
    {
        UIController uiController = FindObjectOfType<UIController>();
        if (uiController == null)
        {
            Debug.LogError("UIController not found in the scene.");
            return;
        }
        Healthbar = uiController.GetHealthbarController();
        AmmoUIController = uiController.GetAmmoUi();
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
    public bool canDie = false;

    int ShieldExtra
    {
        get
        {
            return PlayerPowersSingleton.Instance.ShieldExtra;
        }
    }

    public void SetMaxShield(int maxShield = 0)
    {
        Healthbar.IncMaxShield(maxShield + ShieldExtra);
        // TODO: fix adding empty sheild
        Healthbar.DecFromShield(MissingShield - ShieldExtra);
    }


    public void TakeDamage(int damage)
    {
        // Player dies if hit while shield is broken
        if (shieldIsBroken && canDie)
        {
            Debug.Log("Player Died");
            Healthbar.DecFromShield(1);
            SetPlayerIsDefeated();
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

    void SetPlayerIsDefeated()
    {
        GetComponent<PlayerMovement>().SetPlayerGotDefeated();

        gameObject.GetComponent<BoxCollider2D>().enabled = false;

    }

    void ShowEndScreen()
    {
        // FindAnyObjectByType<EndScreen>()?.PlayInAnim();
        EndScreen.Es.PlayInAnim();
    }

    #endregion





    #region AmmoUI
    // defaults to 0
    public int BulletsAmmo;
    public int ArrowsAmmo;
    bool CanReturnAmmo
    {
        get
        {
            return WeaponPowersSingleton.Instance.ReturnAmmo;
        }
    }
    int maxBullets = 100;
    int maxArrows = 60;

    float MaxMagazineMultiplier
    {
        get
        {
            return PlayerPowersSingleton.Instance.MaxMagazineSizeMultiplier;
        }
    }

    [SerializeField] private AmmunitionUI AmmoUIController;

    public void SetupMaxAmmo()
    {
        // Set the max ammo for the UI
        AmmoUIController.SetMaxAmmo(WeaponType.Bullets, (int)(maxBullets * MaxMagazineMultiplier));
        AmmoUIController.SetMaxAmmo(WeaponType.Arrows, (int)(maxArrows * MaxMagazineMultiplier));
    }

    private void SetAmmo(WeaponType weaponType, int newAmmo)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:

                BulletsAmmo += newAmmo;
                BulletsAmmo = Mathf.Clamp(BulletsAmmo, 0, (int)(maxBullets * MaxMagazineMultiplier));

                AmmoUIController.SetAmmo(WeaponType.Bullets, BulletsAmmo);
                break;
            case WeaponType.Arrows:

                ArrowsAmmo += newAmmo;
                ArrowsAmmo = Mathf.Clamp(ArrowsAmmo, 0, (int)(maxArrows * MaxMagazineMultiplier));

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

        int returnAmmoChance = 7;
        // Return ammo if the power is active
        if (CanReturnAmmo)
        {
            int chance = Random.Range(0, 100);
            if (chance < returnAmmoChance)
            {
                AddAmmo(weaponType, ammo);
                PopUpText.Create(displayText: "Ammo Saved", pos: transform.position, color: PopupColor.White, randomDirection: false);
            }

        }
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

    #region Camera
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;


    public void SetCameraZoom()
    {
        float CameraZoom = PlayerPowersSingleton.Instance.DefaultCameraZoom;
        VirtualCamera.m_Lens.OrthographicSize = CameraZoom;
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
