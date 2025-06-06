using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // rename to player handler
    [SerializeField] private float movementSpeed;

    [SerializeField] private WeaponController weaponsController;

    [SerializeField] bool CanMove = true;
    bool isDefeated = false;

    float MovementSpeedMultiplier
    {
        get
        {
            return PlayerPowersSingleton.Instance.PlayerSpeedMultiplier;
        }
    }

    Rigidbody2D rb;
    PlayerAnim playerAnim;
    PlayerCursor cursorHandler;
    PlayerStats playerStats;

    Vector2 directions;
    Vector2 mousePos;
    Vector2 playerPos;

    public bool flipWeapon = false;

    bool canPickUp;
    public PickupWeapon pickupWeapon;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnim>();
        cursorHandler = GetComponent<PlayerCursor>();
        playerStats = GetComponent<PlayerStats>();

        cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
    }

    void Update()
    {
        if (isDefeated) return;

        // cheat code
        if (Input.GetKeyDown(KeyCode.H))
        {
            playerStats.AddAmmo(WeaponType.Bullets, 100);
            playerStats.AddAmmo(WeaponType.Arrows, 100);
        }
        // movement and animations
        if (CanMove)
        {
            Move();
            Animate();
        }
    }

    void Move()
    {
        rb.velocity = directions * movementSpeed * MovementSpeedMultiplier;
    }

    void Animate()
    {
        if (Time.timeScale == 0) return;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cursorHandler.SetCursorPos(mousePos);
        cursorHandler.SetCursorRot();
        SetWeaponsRot();
        playerPos = transform.position;


        // bool flip = mousePos.x < playerPos.x;
        bool flip = cursorHandler.GetCursorPos().x < playerPos.x;
        if (flipWeapon != flip)
        {
            SetWeaponsControllerPos(flip);
            flipWeapon = flip;
        }
        playerAnim.SetAnim(directions, flip);
    }

    private void SetWeaponsControllerPos(bool flip)
    {
        // int rev = -1
        weaponsController.FlipSprite(flip);
    }

    void SetWeaponsRot()
    {
        // weapon1.RotateSprite(playerPos: playerPos);
        weaponsController.RotateSprite(playerPos: playerPos);
    }
    public void SetPlayerGotDefeated()
    {
        isDefeated = true;
        rb.velocity = Vector2.zero;
        weaponsController.gameObject.SetActive(false);
        // play defeat animation
        playerAnim.SetIsDefeated();
    }

    // input handling area
    #region Input Sys
    public void OnMove(InputAction.CallbackContext context)
    {
        directions = context.ReadValue<Vector2>();
        // Debug.Log("Move called, context:");
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        // Add later for further flexibilty
        // SetCursorPos(context.ReadValue<Vector2>());
        // Debug.Log(context.ReadValue<Vector2>());
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        Debug.Log("Switching weapons");
        if (context.action.WasPressedThisFrame()) weaponsController.SwitchUsedWeapon();
    }
    public void PauseClicked(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            Debug.Log("Pause Clicked");
            PauseScreen.p.PauseGame();
        }

    }

    public void MapToggled(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            Debug.Log("Map Toggled");
            FindObjectOfType<MiniMapController>().ToggleMap();
        }
    }
    #endregion



    public void SetPickupWeapon(PickupWeapon newWeapon, bool canPick)
    {
        pickupWeapon = newWeapon;
        canPickUp = canPick;
        // Debug.Log("Set pickup weapon: " + pickupWeapon + " can pick: " + canPickUp);
    }


    // weapon handling area
    #region Weapons
    bool canUseWeapon = true;
    bool useAmmo = true;
    public void WeaponOneClicked(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || isDefeated) return;
        // Debug.Log("Weapon 1 clicked");
        if (context.action.WasPressedThisFrame())
        {
            if (canPickUp)
            {
                SwitchUsedWeapon();
                // Prevent the player from attacking in the same frame when picking up the weapon
                canUseWeapon = false;
                return;
            }
            if (canUseWeapon)
            {
                WeaponType WT = weaponsController.GetWeapon1Type();

                // the ammo use of the weapon
                int w1Ammo = weaponsController.GetWeapon1Ammo();
                // the ammo the player has
                int ammo = playerStats.GetAmmo(WT);

                // no ammo
                if (ammo <= 0) return;
                // not enough ammo
                if (ammo < w1Ammo) return;



                // fix weapon uses ammo while on weapon cooldown
                if (useAmmo && weaponsController.GetWeaponCanFire())
                {
                    weaponsController.Weapon1Attack();
                    playerStats.DecAmmo(weaponsController.GetWeapon1Type(), w1Ammo);
                    Debug.Log("AmmoUsed: " + w1Ammo);
                    useAmmo = false;
                }
                else
                {
                    useAmmo = true;
                }

                cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
            }
            else
            {
                canUseWeapon = true;
            }
        }

    }

    public void WeaponTwoClicked(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || isDefeated) return;
        // Debug.Log("Weapon 2 clicked");
        if (context.action.WasPressedThisFrame())
        {

            if (canPickUp)
            {
                SwitchUsedWeapon();
                canUseWeapon = false;
                return;
            }
            if (canUseWeapon)
            {
                WeaponType WT = weaponsController.GetWeapon2Type();

                // the ammo use of the weapon
                int w2Ammo = weaponsController.GetWeapon2Ammo();
                // the ammo the player has
                int ammo = playerStats.GetAmmo(WT);

                // no ammo
                if (ammo <= 0) return;
                // not enough ammo
                if (ammo < w2Ammo) return;
                if (useAmmo && weaponsController.GetWeaponCanFire())
                {
                    playerStats.DecAmmo(weaponsController.GetWeapon2Type(), w2Ammo);
                    Debug.Log("AmmoUsed: " + w2Ammo);
                    useAmmo = false;
                }
                else
                {
                    useAmmo = true;
                }

                weaponsController.Weapon2Attack();
                cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
            }
            else
            {
                canUseWeapon = true;
            }
        }

    }

    private void SwitchUsedWeapon()
    {
        AttackingWeapon currentAttack = weaponsController.usedWeapon;
        Debug.Log("Current weapon to switch: " + currentAttack?.weapon.WeaponName);

        weaponsController.SetNewWeapon(pickupWeapon.GetPickWeapon());

        pickupWeapon.SwitchAttackingWeapon(currentAttack);
        canPickUp = false;
    }

    #endregion
}
