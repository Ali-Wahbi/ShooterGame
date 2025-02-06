using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorBullet : MonoBehaviour
{

    float bulletSpeed = 7;
    float bulletDamage;

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
            Destroy(gameObject);
        }
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            //apply damage to enemies
            other.GetComponent<EnemyStats>().TakeDamage((int)bulletDamage);
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position);
            Destroy(gameObject);
        }
    }


    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }
}
