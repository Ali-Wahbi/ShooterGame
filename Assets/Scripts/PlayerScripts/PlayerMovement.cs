using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    [SerializeField] private WeaponController weaponsController;

    [SerializeField] bool CanMove = true;

    Rigidbody2D rb;
    PlayerAnim playerAnim;
    PlayerCursor cursorHandler;

    Vector2 directions;
    Vector2 mousePos;
    Vector2 playerPos;

    public bool flipWeapon = false;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnim>();
        cursorHandler = GetComponent<PlayerCursor>();
        cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
    }

    void Update()
    {
        if (CanMove)
        {
            Move();
            Animate();
        }
    }

    void Move()
    {
        rb.velocity = directions * movementSpeed;
    }

    void Animate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cursorHandler.SetCursorPos(mousePos);
        cursorHandler.SetCursorRot();
        SetWeaponsRot();
        playerPos = transform.position;


        // bool flip = mousePos.x < playerPos.x;
        bool flip = cursorHandler.GetCursorPos().x < playerPos.x;
        if (flipWeapon != flip)
        {
            SetWeaponsControllerPos();
            flipWeapon = flip;
            playerAnim.SetAnim(directions, flip);
        }
    }

    private void SetWeaponsControllerPos()
    {
        int rev = -1;
        Transform wct = weaponsController.GetComponent<Transform>();

        wct.position = wct.position.Multiply(x: rev);
    }

    void SetWeaponsRot()
    {
        // weapon1.RotateSprite(playerPos: playerPos);
        weaponsController.RotateSprite(playerPos: playerPos);
    }

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

    public void PauseClicked()
    {
        Debug.Log("Pause Clicked");

    }
    #endregion







    #region Weapons
    public void WeaponOneClicked(InputAction.CallbackContext context)
    {
        // Debug.Log("Weapon 1 clicked");
        if (context.action.WasPressedThisFrame())
        {

            weaponsController.Weapon1Attack();
            cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
        }

    }

    public void WeaponTwoClicked(InputAction.CallbackContext context)
    {
        // Debug.Log("Weapon 2 clicked");
        if (context.action.WasPressedThisFrame())
        {

            weaponsController.Weapon2Attack();
            cursorHandler.SetCursorSprite(weaponsController.GetWeaponType());
        }

    }
    #endregion
}
