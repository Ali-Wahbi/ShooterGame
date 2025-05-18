using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exentsions;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Breakable : MonoBehaviour
{
    [Tooltip("Normal sprite to display")]
    [SerializeField] Sprite Normal;

    [Tooltip("Duration of the flash effect")]
    [SerializeField, Range(0, 1)] float FlashDuration;
    [SerializeField] Material FlashMaterial;
    private Material OriginalMaterial;

    [Tooltip("Sprites of the object when it is broke. Pick randomly")]
    [SerializeField] List<Sprite> Broken;

    BoxCollider2D breakableCollider;
    SpriteRenderer SR;

    int spawnDropChance = 10;
    int weaponDropChance = 5;
    private void Reset()
    {
        gameObject.tag = "Breakable";
        SetCollider();
        SetRigidBody();
    }
    // Start is called before the first frame update
    void Start()
    {
        breakableCollider = GetComponent<BoxCollider2D>();
        SR = GetComponent<SpriteRenderer>();
        AtStart();


        OriginalMaterial = SR.material;
    }

    public void OnBreak()
    {
        DisableCollider();

        FlashHitEffect();

        SpawnDrops();
        // small exlposion then spawn drops
    }

    private void DisableCollider()
    {
        breakableCollider.enabled = false;
    }

    private void SetRandomSprite()
    {
        // Sprite rand = Random
        int randomChoice = Random.Range(0, Broken.Count);
        Sprite randomSprite = Broken[randomChoice];
        SR.sprite = randomSprite;
    }

    private void SpawnDrops()
    {
        // spawn ammo
        int chance = Random.Range(0, 100);
        if (chance < spawnDropChance)
        {
            // spawn small ammo
            Debug.Log("Spawn ammo");
            Instantiate(GameAssets.g.AmmoPickUpPrefap, position: transform.position, rotation: Quaternion.identity);
            return;
        }

        if (chance < weaponDropChance)
        {
            // spawn a weapon
            Debug.Log("Enemy Spawn weapon");
            Instantiate(GameAssets.g.pickUpWeaponPrefap, position: transform.position.With(z: -0.1f), rotation: Quaternion.identity);
            return;
        }
    }

    public void MultDropChance(float amount)
    {
        spawnDropChance = (int)(spawnDropChance * amount);
    }

    public void AtStart()
    {
        breakableCollider.enabled = true;
        SR.sprite = Normal;
    }


    void FlashHitEffect()
    {
        StartCoroutine(FlashEffect());

    }

    private IEnumerator FlashEffect()
    {
        SR.material = FlashMaterial;
        yield return new WaitForSeconds(FlashDuration);
        SR.material = OriginalMaterial;


        SetRandomSprite();
    }


    #region Settings

    void SetRigidBody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    void SetCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        collider.offset = new Vector2(0, -0.6f);
        collider.size = new Vector2(1, 0.7f);
    }


    #endregion
}
