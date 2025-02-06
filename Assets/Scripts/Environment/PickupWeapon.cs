using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] AttackingWeapon pickWeapon;
    SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        SetSprite();

        pickWeapon.canFire = true;
        pickWeapon = Instantiate(pickWeapon, transform);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && pickWeapon)
        {
            OutlineVisability(visible: true);
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.SetPickupWeapon(this, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && pickWeapon)
        {
            OutlineVisability(visible: false);
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.SetPickupWeapon(null, false);
        }
    }

    void OutlineVisability(bool visible)
    {
        int vis = visible ? 1 : 0;
        GetComponent<Renderer>().material.SetInt("_ShowOutline", vis);
    }

    void SetSprite() => sp.sprite = pickWeapon.GetWeaponSprite();

    public AttackingWeapon GetPickWeapon() => pickWeapon;



    public void SwitchAttackingWeapon(AttackingWeapon newAttackWeapon)
    {
        if (newAttackWeapon == null)
        {
            pickWeapon = null;
            sp.sprite = null;
            return;
        }
        else
        {
            pickWeapon = newAttackWeapon;
            SetSprite();
        }
    }

}
