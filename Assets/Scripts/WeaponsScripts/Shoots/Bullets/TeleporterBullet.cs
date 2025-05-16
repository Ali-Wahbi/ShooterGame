using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer sp;
    // SpriteRenderer sp;
    float bulletSpeed = 7;

    public void SetBulletSpeedAndDamage(float speed, float damage)
    {
        bulletSpeed = speed;

    }
    //  Detect the bullet collision
    private void OnTriggerEnter2D(Collider2D other)
    {

    }
    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
        bulletSpeed -= Time.deltaTime * 2f;
    }
    void DestroySelf(float delay)
    {
        bulletSpeed = 0;
        GetComponent<BoxCollider2D>().enabled = false;
        sp.enabled = false;
        Destroy(gameObject, delay);
    }
}
