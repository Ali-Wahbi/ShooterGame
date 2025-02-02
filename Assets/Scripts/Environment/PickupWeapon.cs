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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && pickWeapon)
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.SetPickupWeapon(this, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && pickWeapon)
        {
            PlayerMovement pm = other.gameObject.GetComponent<PlayerMovement>();
            pm.SetPickupWeapon(null, false);
        }
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
