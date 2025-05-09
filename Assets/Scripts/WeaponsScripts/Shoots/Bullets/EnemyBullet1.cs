using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    [SerializeField] private GameObject PlayerBulletPrefab;
    // Refactor bullets
    float bulletSpeed = 7;
    float bulletDamage = 2;
    int timesReflected = 0;

    public void SetBulletSpeedAndDamage(float speed, float damage, int timesReflectedNew = 0)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        timesReflected = timesReflectedNew;
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
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
            other.gameObject.GetComponent<Breakable>().OnBreak();
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

    public void ReflectBullet(float newZAngle)
    {
        if (PlayerBulletPrefab == null) return;
        if (timesReflected >= 3) return;

        GameObject bult = Instantiate(PlayerBulletPrefab, position: transform.position, Quaternion.identity);
        bult.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, newZAngle);

        //Set up
        bult.GetComponent<VectorBullet>().SetBulletSpeedAndDamage(bulletSpeed * 0.8f, bulletDamage, timesReflected + 1);
    }


}
