
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public AttackingWeapon usedWeapon;
    public AttackingWeapon weapon1;
    public AttackingWeapon weapon2;
    public SpriteRenderer GunSprite;
    public SpriteRenderer SlashSprite;
    // public SpriteRenderer StapSprite;
    public Transform cursor;
    public Transform bulletPointer;

    private void Start()
    {
        SetUsedWeapon(weapon: weapon1);
        SetSprite();
    }
    void SetUsedWeapon(AttackingWeapon weapon)
    {
        usedWeapon = weapon;
        SetSprite();
    }
    public void Weapon1Attack()
    {
        if (weapon1)
        {
            SetUsedWeapon(weapon1);
            if (usedWeapon) Attack();
        }
    }
    public void Weapon2Attack()
    {
        if (weapon2)
        {
            SetUsedWeapon(weapon2);
            if (usedWeapon) Attack();
        }
    }
    void Attack()
    {
        if (usedWeapon.canFire)
        {
            Transform spriteTransform = GetSpriteTransform();

            // Refactor this dynamic to be the main attack
            usedWeapon.DynamicAttack(startTran: bulletPointer, startRot: transform, sprite: spriteTransform);

        }

    }

    Transform GetSpriteTransform()
    {
        switch (usedWeapon.GetWeaponAnim())
        {
            case WeaponAnim.Stap:
                return null;

            case WeaponAnim.Slash:
                return SlashSprite.GetComponent<Transform>();

            case WeaponAnim.Shoot:
                return GunSprite.GetComponent<Transform>();

            default:
                return null;
        }
    }

    public void RotateSprite(Vector2 playerPos)
    {
        Vector2 cursorPos = cursor.transform.position;
        // Transform of the cursor
        float angle = Mathf.Atan2(playerPos.y - cursorPos.y, playerPos.x - cursorPos.x);
        float correctAngle = angle * Mathf.Rad2Deg + 180;

        // set the cursor rotation
        transform.rotation = Quaternion.Euler(0, 0, correctAngle);

        // set the vertical flip of the sprite
        GunSprite.flipY = correctAngle > 90 && correctAngle < 270;
    }

    public void SetWeapon(Weapon weapon)
    {
        usedWeapon.weapon = weapon;
        SetSprite();
    }

    void SetSprite()
    {
        if (usedWeapon)
        {
            switch (usedWeapon.GetWeaponAnim())
            {
                case WeaponAnim.Stap:
                    break;
                case WeaponAnim.Slash:
                    SlashSprite.enabled = true;
                    GunSprite.enabled = false;
                    SlashSprite.sprite = usedWeapon.GetWeaponSprite();
                    break;

                case WeaponAnim.Shoot:
                    GunSprite.enabled = true;
                    SlashSprite.enabled = false;
                    GunSprite.sprite = usedWeapon.GetWeaponSprite();
                    break;

                default:
                    break;
            }
        }
        else
        {
            GunSprite.sprite = null;
            SlashSprite.sprite = null;
        }

    }

    public WeaponType GetWeaponType() => usedWeapon ? usedWeapon.GetWeaponType() : WeaponType.Bullets;

}
