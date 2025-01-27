using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;

    [SerializeField] private WeaponController weapon1;
    [SerializeField] private WeaponController weapon2;

    public WeaponType weapon1Type;
    public WeaponType weapon2Type;
    Rigidbody2D rb;
    PlayerAnim playerAnim;
    PlayerCursor cursorHandler;

    Vector2 directions;
    Vector2 mousePos;
    Vector2 playerPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<PlayerAnim>();
        cursorHandler = GetComponent<PlayerCursor>();
        cursorHandler.SetCursorSprite(weapon1Type);
    }

    void Update()
    {
        Move();
        Animate();
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


        bool flip = mousePos.x < playerPos.x;
        playerAnim.SetAnim(directions, flip);
    }

    void SetWeaponsRot()
    {
        weapon1.RotateSprite(playerPos: playerPos);
        weapon2.RotateSprite(playerPos: playerPos);
    }

    #region Input Sys
    public void OnMove(InputAction.CallbackContext context)
    {
        directions = context.ReadValue<Vector2>();
        Debug.Log("Move called, context:");
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        // Add later for further flexibilty
        // SetCursorPos(context.ReadValue<Vector2>());
        // Debug.Log(context.ReadValue<Vector2>());
    }
    #endregion







    #region Weapons
    public void WeaponOneClicked()
    {
        // Debug.Log("Weapon 1 clicked");
        cursorHandler.SetCursorSprite(weapon1.GetWeaponType());
        weapon1.Attack();

    }

    public void WeaponTwoClicked()
    {
        // Debug.Log("Weapon 2 clicked");
        cursorHandler.SetCursorSprite(weapon2.GetWeaponType());
        weapon2.Attack();

    }
    #endregion

    public void PauseClicked()
    {
        Debug.Log("Pause Clicked");

    }
}
