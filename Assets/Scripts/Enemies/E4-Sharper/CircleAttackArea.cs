using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackArea : MonoBehaviour
{

    int damage;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<PlayerStats>().TakeDamage(damage);
        }

        VectorBullet bullet;
        if (collision.gameObject.TryGetComponent(out bullet))
        {
            bullet.ReflectBullet();
            // bullet.ReflectBullet(startPoint: transform.position, endPoint: collision.transform.position);

        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}
