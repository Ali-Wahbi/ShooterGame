using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberSword : SlashWeapon
{
    bool isCharged = false;
    [SerializeField] Sprite chargedSprite;
    [SerializeField] Sprite chargedSpriteFlipped;
    public override void DynamicSlash(Transform startTran, Transform startRot)
    {
        DischargeSword();
        // instantaite the slash here 
        GameObject _slash = Instantiate(slash, startTran.position, startRot.rotation);

        int damage = (int)GetWeaponDamage();
        if (isCharged) damage *= 2;

        _slash.GetComponent<SlashEffect>().Setup(damage: damage);
        _slash.GetComponent<AbsorberSlash>().onBulletHitEvent.AddListener(OnSlashHitBullet);

    }


    void OnSlashHitBullet()
    {
        ChargeSword();
    }

    void ChargeSword()
    {
        // Debug.Log("Absorber is chraged!");
        isCharged = true;
        Sprite sprite = isFliped ? chargedSpriteFlipped : chargedSprite;
        SetSprite(sprite: sprite);
    }

    void DischargeSword()
    {
        if (!isCharged) return;
        isCharged = false;
        Sprite sprite = isFliped ? MeleeFlibed : MeleeNormal;
        SetSprite(sprite: sprite);
    }

    void SetSprite(Sprite sprite)
    {
        WeaponController WC = FindFirstObjectByType<WeaponController>();
        if (WC)
        {
            WC.SetMeleeSprite(sprite);
            Debug.Log("Absorber changed to " + sprite.name);
        }
        else { Debug.Log("Could not find WC"); }

    }
}
