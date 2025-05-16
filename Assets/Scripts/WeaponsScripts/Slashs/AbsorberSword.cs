using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorberSword : SlashWeapon
{
    bool isCharged = false;
    [SerializeField] public GameObject AbsorberSlash;
    [SerializeField] Sprite chargedSprite;
    [SerializeField] Sprite chargedSpriteFlipped;
    Sprite normalSPrite, normalSPriteFlip;
    WeaponController WC;
    private void Start()
    {
        WC = FindFirstObjectByType<WeaponController>();
        normalSPrite = MeleeNormal;
        normalSPriteFlip = MeleeFlibed;
    }
    public override void DynamicSlash(Transform startTran, Transform startRot)
    {
        int damage = (int)GetWeaponDamage();
        if (isCharged)
        {
            // Charged slash is bigger, blue, and deals more damage.
            damage *= 2;
            GameObject _slash = Instantiate(AbsorberSlash, startTran.position, startRot.rotation);
            _slash.GetComponent<SlashEffect>().Setup(damage: damage);
            _slash.GetComponent<AbsorberSlash>().SetParent(this);
        }
        else
        {
            // normal slash is like the sword slash.
            GameObject _slash = Instantiate(slash, startTran.position, startRot.rotation);
            _slash.AddComponent<AbsorberSlash>().SetParent(this);
            _slash.GetComponent<SlashEffect>().Setup(damage: damage);

        }

    }


    public void OnSlashHitBullet()
    {
        ChargeSword();
    }

    public void OnSlashMissedBullet()
    {
        DischargeSword();
    }

    void ChargeSword()
    {
        isCharged = true;

        Sprite sprite = isFliped ? chargedSpriteFlipped : chargedSprite;
        SetSprite(sprite: sprite);

        if (isFliped) MeleeFlibed = chargedSpriteFlipped;
        else MeleeNormal = chargedSprite;

    }

    void DischargeSword()
    {
        if (!isCharged) return;
        isCharged = false;

        Sprite sprite = isFliped ? MeleeFlibed : MeleeNormal;
        SetSprite(sprite: sprite);

        if (isFliped) MeleeFlibed = normalSPriteFlip;
        else MeleeNormal = normalSPrite;
    }

    void SetSprite(Sprite sprite)
    {

        if (WC)
            WC.SetMeleeSprite(sprite);

        else { Debug.Log("Could not find WC"); }

    }
}
