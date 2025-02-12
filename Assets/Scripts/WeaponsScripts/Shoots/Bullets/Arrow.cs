using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    protected float bulletSpeed = 7;
    float bulletDamage;
    int pierceCounter = 3;
    int currentPierce = 0;
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
            DestroyArrow();
        }
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            DestroyArrow();
        }
        if (other.gameObject.tag == "Enemy")
        {
            //apply damage to enemies
            other.GetComponent<EnemyStats>().TakeDamage((int)bulletDamage);
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position);

            // increase the piercing counter
            currentPierce++;
            if (currentPierce >= pierceCounter) DestroyArrow();
        }
    }


    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }

    protected void DestroyArrow()
    {
        bulletSpeed = 0;
        Destroy(gameObject, 1f);
    }
}
