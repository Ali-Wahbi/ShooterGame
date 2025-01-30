using System.Collections;
using UnityEngine;

abstract public class SlashWeapon : AttackingWeapon
{
    bool isFlibed = false;
    float rotateDeg = 120;

    [SerializeField] Sprite MeleeNormal;
    [SerializeField] Sprite MeleeFlibed;

    [SerializeField] public GameObject slash;
    public override void DynamicAttack(Transform startTran, Transform startRot, Transform sprite)
    {
        canFire = false;
        DynamicSlash(startTran, startRot);
        // sprite.position.Add(x: 2);
        AnimateSlash(sprite);
        Timer();
    }

    abstract public void DynamicSlash(Transform startTran, Transform startRot);

    void Timer()
    {
        StartCoroutine(StartTimer());
    }

    // Fire rate timer
    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(GetWeaponReloadSpeed());
        // Debug.Log("Timer ended");
        canFire = true;

    }

    void AnimateSlash(Transform sprite)
    {
        StartCoroutine(AnimateSlashing(sprite));
    }


    IEnumerator AnimateSlashing(Transform sprite)
    {
        // sprite.position = new Vector3(sprite.position.x - 1f, sprite.position.y, sprite.position.z);
        int one = isFlibed ? 1 : -1;
        sprite.Rotate(0, 0, one * rotateDeg);
        isFlibed = !isFlibed;

        // Debug.Log("Animating slashing: " + sprite);
        yield return new WaitForSeconds(0.3f);
        SetWeaponSprite(sprite);
    }

    void SetWeaponSprite(Transform sprite)
    {
        SpriteRenderer sr = sprite.GetComponent<SpriteRenderer>();

        if (sr)
        {
            if (isFlibed)
            {
                sr.sprite = MeleeFlibed;
            }
            else
            {
                sr.sprite = MeleeNormal;
            }
        }
    }
}
