using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinumBullet : MonoBehaviour
{
    [SerializeField] Sprite zero, one;
    [SerializeField] SpriteRenderer sp;
    // SpriteRenderer sp;
    float bulletSpeed = 7;
    float bulletDamage;

    public void SetBulletSpeedAndDamage(float speed, float damage)
    {
        bulletSpeed = speed;
        bulletDamage = damage;

        // rotate the sprite so it always appear correctly
        sp.transform.Rotate(new Vector3(0, 0, -transform.eulerAngles.z));
        if (damage == 0)
            sp.sprite = zero;
        else
            sp.sprite = one;


    }
    //  Detect the bullet collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if (other.gameObject.tag == "Wall")
        // {
        //     DestroySelf();
        // }
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            DestroySelf(2f);
        }
        if (other.gameObject.tag == "Enemy")
        {
            //apply damage to enemies
            other.GetComponent<EnemyStats>().TakeDamage((int)bulletDamage);
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position);
            // DestroySelf();
        }
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }
    void DestroySelf(float delay)
    {
        bulletSpeed = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        sp.enabled = false;
        Destroy(gameObject, delay);
    }
}
