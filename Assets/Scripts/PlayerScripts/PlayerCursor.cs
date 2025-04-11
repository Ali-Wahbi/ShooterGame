using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCursor : MonoBehaviour
{


    [SerializeField] private GameObject cursor;
    [SerializeField] private float cursorSensitivity = 0.1f;
    [Header("Sprites: ")]
    [SerializeField] SpriteRenderer cursorSprite;



    public Sprite BulletSprite;
    public Sprite ArrowSprite;
    public Sprite MeleeSprite;

    #region Cursor

    private void Start()
    {
        // Hide the pointer
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }


    public void SetCursorPos(Vector2 pos)
    {
        cursor.transform.position = pos * cursorSensitivity;
    }
    public Vector3 GetCursorPos()
    {
        return cursor.transform.position;
    }
    // Rotate the cursor based on angle from player to cursor
    public void SetCursorRot()
    {
        // get the two vector2 positions of the player and cursor
        Vector2 playerPos = transform.position;
        Vector2 curPos = cursor.transform.position;

        // calculate the angle from the player position to the cursor position
        float angle = Mathf.Atan2(playerPos.y - curPos.y, playerPos.x - curPos.x);
        float correctAngle = angle * Mathf.Rad2Deg + 180;

        // set the cursor rotation
        cursor.transform.rotation = Quaternion.Euler(0, 0, correctAngle);
    }

    // Change the cursor sprite based on shooted weapon
    public void SetCursorSprite(WeaponType weaponType = WeaponType.Bullets)
    {
        switch (weaponType)
        {
            case WeaponType.Bullets:
                SetSprite(BulletSprite);
                break;

            case WeaponType.Arrows:
                SetSprite(ArrowSprite);
                break;
            case WeaponType.Melee:
                SetSprite(MeleeSprite);
                break;

            default:
                break;
        }
    }

    void SetSprite(Sprite sprite)
    {
        cursorSprite.sprite = sprite;
    }
    #endregion
}
