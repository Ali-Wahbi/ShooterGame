
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
            usedWeapon.DynamicAttack(
                startTran: bulletPointer,
                startRot: transform,
                sprite: spriteTransform,
                CursorPos: cursor.position);

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

    public void SetNewWeapon(AttackingWeapon pickupWeapon)
    {
        // usedWeapon.weapon = pickupWeapon;
        if (usedWeapon == weapon1)
        {
            weapon1 = pickupWeapon;
            SetUsedWeapon(weapon1);
            return;
        }

        if (usedWeapon == weapon2)
        {
            weapon2 = pickupWeapon;
            SetUsedWeapon(weapon2);
            return;
        }
    }

    public WeaponType GetWeapon1Type() => weapon1 ? weapon1.weapon.WeaponType : WeaponType.Melee;
    public int GetWeapon1Ammo() => weapon1 ? weapon1.weapon.WeaponAmmo : 0;
    public bool GetWeaponIsAutomatic() => weapon1 ? weapon1.GetWeaponIsAutomatic() : false;
    public WeaponType GetWeapon2Type() => weapon2 ? weapon2.weapon.WeaponType : WeaponType.Melee;
    public int GetWeapon2Ammo() => weapon2 ? weapon2.weapon.WeaponAmmo : 0;

    public bool GetWeaponCanFire() => usedWeapon ? usedWeapon.GetWeaponCanFire() : false;

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

    public void FlipSprite(bool flip)
    {
        float newX = flip ? 0.19f : -0.19f;
        transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
    }

    public void SwitchUsedWeapon()
    {
        if (usedWeapon == weapon1)
        {

            weapon1 = weapon2;
            weapon2 = usedWeapon;
            SetUsedWeapon(weapon1);
            return;
        }
        if (usedWeapon == weapon2)
        {
            weapon2 = weapon1;
            weapon1 = usedWeapon;
            SetUsedWeapon(weapon2);
            return;
        }
    }
    public WeaponType GetWeaponType() => usedWeapon ? usedWeapon.GetWeaponType() : WeaponType.Bullets;

}
