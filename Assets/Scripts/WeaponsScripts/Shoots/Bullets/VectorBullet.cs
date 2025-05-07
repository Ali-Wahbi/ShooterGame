using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorBullet : MonoBehaviour
{
    [SerializeField] private GameObject EnemyBulletPrefab;
    float bulletSpeed = 7;
    float bulletDamage;
    int timesReflected = 0;

    public void SetBulletSpeedAndDamage(float speed, float damage, int timesReflectedNew = 0)
    {
        bulletSpeed = speed;
        bulletDamage = damage;
        timesReflected = timesReflectedNew;
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
            PopUpText.Create(damage: (int)bulletDamage, pos: transform.position);
            DestroySelf();
        }
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
    }

    /// <summary>
    /// reflect the bullet with reverse direction
    /// </summary>
    public void ReflectBullet()
    {
        float newZAngle = transform.eulerAngles.z + 180f;
        InstantiateReverseBullet(newZAngle);
    }


    /// <summary>
    /// reflect the bullet with the given direction
    /// </summary>
    /// <param name="newZAngle">the angle of the reflected bullet</param>
    public void ReflectBullet(float newZAngle)
    {
        InstantiateReverseBullet(newZAngle);
    }

    /// <summary>
    /// reflect the bullet with direction calculated from the given points
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endPoint"></param>
    public void ReflectBullet(Vector3 startPoint, Vector3 endPoint)
    {
        float newZAngle = CalculateTanAngle(startPoint, endPoint) * Mathf.Rad2Deg;
        InstantiateReverseBullet(newZAngle);

    }

    private void InstantiateReverseBullet(float newZAngle)
    {
        if (EnemyBulletPrefab == null) return;
        if (timesReflected >= 3) return;

        GameObject bult = Instantiate(EnemyBulletPrefab, position: transform.position, Quaternion.identity);
        bult.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, newZAngle);

        //Set up
        bult.GetComponent<EnemyBullet>().SetBulletSpeedAndDamage(bulletSpeed * 0.9f, bulletDamage, timesReflected + 1);
    }

    float CalculateTanAngle(Vector3 startPoint, Vector3 endPoint)
    {

        float zAngle;
        zAngle = Mathf.Atan2(endPoint.x - startPoint.x, endPoint.y - startPoint.y);
        return zAngle;
    }

    void DestroySelf()
    {
        bulletSpeed = 0;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
