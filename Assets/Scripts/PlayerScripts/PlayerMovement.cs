using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
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
    PlayerInputs playerInput;
    Vector2 directions;
    Vector2 mousePos;
    Vector2 playerPos;

    public bool flipWeapon = false;

    bool canPickUp;
    public PickupWeapon pickupWeapon;

    private void Awake()
    {
        playerInput = new PlayerInputs();
    }


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

    void OnEnables()
    {

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found in the scene.");
            return;
        }
        playerInput.Enable();

        playerInput.Movement.Move.performed += OnMove;
        playerInput.Movement.Aim.performed += OnAim;
        playerInput.Movement.Pause.performed += PauseClicked;
        playerInput.Movement.SwitchWeapons.performed += OnSwitch;
        playerInput.Movement.ToggleMap.performed += MapToggled;

        playerInput.Movement.Weapon1.performed += WeaponOneClicked;
        playerInput.Movement.Weapon2.performed += WeaponTwoClicked;

        // playerInput.Movement
        // playerInput.actions.actionMaps[0].;
    }

    private void OnDisables()
    {
        playerInput.Movement.Move.performed -= OnMove;
        playerInput.Movement.Aim.performed -= OnAim;
        playerInput.Movement.Pause.performed -= PauseClicked;
        playerInput.Movement.SwitchWeapons.performed -= OnSwitch;
        playerInput.Movement.ToggleMap.performed -= MapToggled;

        playerInput.Movement.Weapon1.performed -= WeaponOneClicked;
        playerInput.Movement.Weapon2.performed -= WeaponTwoClicked;

        playerInput.Disable();
    }


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
    public bool weapon1Attcking;
    public bool weapon2Attcking;
    Coroutine weaponAttackCoroutine;
    public void WeaponOneClicked(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0 || isDefeated) return;
        if (weapon2Attcking) return;
        weapon1Attcking = true;
        // Debug.Log("Weapon 1 clicked");

        // if (context.action.WasPressedThisFrame()) 
        if (context.action.WasReleasedThisFrame())
        {
            weapon1Attcking = false;
            weaponAttackCoroutine = null;
        }
        Debug.Log("Weapon 1 Attacking, weapon1Attcking " + weapon1Attcking);
        if (weapon1Attcking)
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

                // fix weapon uses ammo while on weapon cooldown
                if (useAmmo && weaponsController.GetWeaponCanFire() && !weapon2Attcking)
                {
                    if (weaponsController.GetWeapon1IsAutomatic())
                    {
                        Debug.Log($"Weapon one {weaponsController.GetWeapon1Type()} is automatic");
                        if (weaponAttackCoroutine == null)
                            weaponAttackCoroutine = StartCoroutine(weaponOneAttackCoroutine());
                    }
                    else AttackWithWeaponOne();
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
        if (weapon1Attcking) return;
        weapon2Attcking = true;

        if (context.action.WasReleasedThisFrame())
        {
            weapon2Attcking = false;
            weaponAttackCoroutine = null;
        }
        Debug.Log("Weapon 2 Attacking, weapon2Attcking " + weapon2Attcking);

        // Debug.Log("Weapon 2 clicked");
        if (weapon2Attcking)
        {

            if (canPickUp)
            {
                SwitchUsedWeapon();
                canUseWeapon = false;
                return;
            }
            if (canUseWeapon)
            {

                if (useAmmo && weaponsController.GetWeaponCanFire() && !weapon1Attcking)
                {
                    if (weaponsController.GetWeapon2IsAutomatic())
                    {
                        Debug.Log("Weapon two is automatic");
                        if (weaponAttackCoroutine == null)
                            weaponAttackCoroutine = StartCoroutine(weaponTwoAttackCoroutine());
                    }
                    else { AttackWithWeaponTwo(); Debug.Log("Weapon 2 is NOT automatic"); }
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

    private void AttackWithWeaponOne()
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
        weaponsController.Weapon1Attack();
        playerStats.DecAmmo(weaponsController.GetWeapon1Type(), w1Ammo);
        Debug.Log("AmmoUsed: " + w1Ammo);
        useAmmo = false;
    }
    private void AttackWithWeaponTwo()
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
        weaponsController.Weapon2Attack();
        playerStats.DecAmmo(weaponsController.GetWeapon2Type(), w2Ammo);
        Debug.Log("AmmoUsed: " + w2Ammo);
        useAmmo = false;
    }

    IEnumerator weaponOneAttackCoroutine()
    {
        // Debug.Log("weapon1Coroutine is started: " + weaponAttackCoroutine);
        while (weapon1Attcking)
        {
            // Debug.Log("weapon1Coroutine is working");
            AttackWithWeaponOne();
            yield return new WaitForSeconds(weaponsController.GetWeapon1Speed());
        }
    }
    IEnumerator weaponTwoAttackCoroutine()
    {
        // Debug.Log("weapon2Coroutine is started: " + weaponAttackCoroutine);
        while (weapon2Attcking)
        {
            // Debug.Log("weapon2Coroutine is working");
            AttackWithWeaponTwo();
            yield return new WaitForSeconds(weaponsController.GetWeapon2Speed());
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
