using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet1 : MonoBehaviour
{
    // Refactor bullets
    float bulletSpeed = 7;
    float bulletDamage = 2;

    public void SetBulletSpeedAndDamage(float speed, float damage)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
    }

    private void Start()
    {
        Destroy(gameObject, 7f);
    }

    //  Detect the bullet collision
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Breakable")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerStats>().TakeDamage((int)bulletDamage);
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position, color: PopupColor.Red);
            Destroy(gameObject);
        }

    }


    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }
}
