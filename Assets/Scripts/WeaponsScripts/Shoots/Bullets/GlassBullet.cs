using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 7;
    [SerializeField] float bulletDamage = 4;

    public void SetBulletSpeedAndDamage(float speed, float damage)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
    }
    //  Detect the bullet collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            DestroySelf();
        }
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            DestroySelf();
        }
        if (other.gameObject.tag == "Enemy")
        {
            //apply damage to enemies
            other.GetComponent<EnemyStats>().TakeDamage((int)bulletDamage);
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position, color: PopupColor.Cyan);
            DestroySelf();
        }
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }

    void DestroySelf()
    {
        bulletSpeed = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
