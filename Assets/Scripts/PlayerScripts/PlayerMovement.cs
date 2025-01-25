using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject cursor;
    [SerializeField] private float cursorSensitivity = 0.1f;
    PlayerAnim pAnimator;

    // horizontal and vertical movement for the player
    float horizon;
    float vertic;

    Vector2 directions;
    Vector2 mousePos;
    Vector2 playerPos;

    void Start()
    {
        pAnimator = GetComponent<PlayerAnim>();
    }

    void Update()
    {
        Move();
        Animate();
    }

    void Move()
    {
        characterController.Move(directions * movementSpeed * Time.deltaTime);
    }

    void Animate()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        SetCursorPos(mousePos);
        playerPos = transform.position;


        bool flip = mousePos.x < playerPos.x;
        pAnimator.SetAnim(directions, flip);
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

    #region Cursor
    void SetCursorPos(Vector2 pos)
    {
        cursor.transform.position = pos * cursorSensitivity;
    }

    // Rotate the cursor based on angle from player to cursor
    void SetCursorRot()
    {

    }

    // Change the cursor sprite based on shooted weapon
    void SetCursorSprite()
    {

    }
    #endregion

    #region Weapons
    public void WeaponOneClicked()
    {
        Debug.Log("Weapon 1 clicked");
    }

    public void WeaponTwoClicked()
    {
        Debug.Log("Weapon 2 clicked");

    }
    #endregion

    public void PauseClicked()
    {
        Debug.Log("Pause Clicked");

    }
}
