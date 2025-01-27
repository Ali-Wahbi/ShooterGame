
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public AttackingWeapon usedWeapon;
    public SpriteRenderer sprite;
    public Transform cursor;
    public Transform bulletPointer;

    private void Start()
    {
        SetSprite();
    }
    public void Attack()
    {
        if (usedWeapon.canFire)
        {
            Transform spriteTransform = sprite.GetComponent<Transform>();
            // usedWeapon.Attack(startPos: bulletPointer.position, startRot: transform.rotation);
            // try dynamic attack
            // Refactor this dynamic to be the main attack
            usedWeapon.DynamicAttack(startTran: bulletPointer, startRot: transform);
            AttackAnim(spriteTransform);
        }

    }

    void AttackAnim(Transform anim)
    {

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
        sprite.flipY = correctAngle > 90 && correctAngle < 270;
    }
    public void Animate() { }

    public void SetWeapon(Weapon weapon)
    {
        usedWeapon.weapon = weapon;
        SetSprite();
    }

    void SetSprite()
    {
        sprite.sprite = usedWeapon.GetWeaponSprite();
    }

    public WeaponType GetWeaponType()
    {
        return usedWeapon.GetWeaponType();
    }

}
