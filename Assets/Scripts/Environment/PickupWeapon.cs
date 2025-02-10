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
        // randomize the wepon for the player
        GetRandomWeapon();
        // Set the ouline color based on the weapon class
        SetOutlineColor();
        // rotate the weapon randomlly
        SetRandomRotation();

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

    #region  Outline
    void OutlineVisability(bool visible)
    {
        int vis = visible ? 1 : 0;
        GetComponent<Renderer>().material.SetInt("_ShowOutline", vis);
    }

    void OutlineColor(Color color)
    {
        // Set _OutlineColor of the material
        GetComponent<Renderer>().material.SetColor("_OutlineColor", color);
    }

    void SetOutlineColor()
    {
        Color color;

        switch (pickWeapon.GetWeaponClass())
        {
            case WeaponClasses.White:
                color = Color.white;
                break;
            case WeaponClasses.Green:
                color = Color.green;
                break;
            case WeaponClasses.Orange:
                color = Color.red;
                break;
            default:
                color = Color.white;
                break;
        }

        OutlineColor(color);
    }

    #endregion

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


    #region set up

    string SlashWeapons = "Slashes";
    string ShootWeapons = "Shoots";
    string PreFolder = "Prefabs/Weapons/";
    List<string> AllPaths = new List<string>();



    void FillPathsList()
    {
        AllPaths.Clear();

        AllPaths.Add(SlashWeapons);
        AllPaths.Add(ShootWeapons);
    }

    string PickRandomFolder()
    {
        int index = Random.Range(0, AllPaths.Count);
        return AllPaths[index];
    }

    void GetRandomWeapon()
    {
        // Fill the list of paths
        FillPathsList();

        // Pick a random folder for the weapon
        string folder = PickRandomFolder();

        string fullPath = PreFolder + folder;
        GameObject[] AllWeapons = Resources.LoadAll<GameObject>(fullPath);

        // Pick a random weapon
        int ranIndex = Random.Range(0, AllWeapons.Length);
        GameObject weapon = AllWeapons[ranIndex];

        // assign the weapon to the pickweapon
        AttackingWeapon attackingWeapon = weapon.GetComponent<AttackingWeapon>();
        if (attackingWeapon) pickWeapon = attackingWeapon;
    }

    void SetRandomRotation()
    {
        float randRot = Random.Range(0f, 90f);

        transform.Rotate(0, 0, randRot);
    }
    #endregion

}
