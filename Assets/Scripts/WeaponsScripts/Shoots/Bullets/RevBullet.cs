using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevBullet : MonoBehaviour
{
    [SerializeField] Transform childTransform;

    float bulletSpeed = 7;
    float bulletDamage;

    Vector2 endPos;
    float distance;

    public void SetBulletSpeedDamagePos(float speed, float damage, Vector2 pos)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        endPos = pos;
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
        childTransform.Rotate(0, 0, 90 * Time.deltaTime * 2);
        transform.position += transform.right * Time.deltaTime * bulletSpeed;

        // calcualte the distace between the startTran and CursorPos
        distance = Vector2.Distance(transform.position, endPos);
        if (distance <= 0.7f)
        {
            Destroy(gameObject);
        }
    }
}
