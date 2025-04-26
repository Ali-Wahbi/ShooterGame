using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicWeapon : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawnPoint;
    int damage;
    [SerializeField] bool canAttack = false;

    SpriteRenderer GunSprite;
    // Start is called before the first frame update
    void Start()
    {
        GunSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && canAttack) RotateSprite();

    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
    public void SetCanAttack(bool canAttack)
    {
        this.canAttack = canAttack;
        FireBullets();
    }


    public void FireBullets()
    {
        StartCoroutine(ShootBullets(bulletSpawnPoint));
    }


    IEnumerator ShootBullets(Transform startTran)
    {
        float bulletSpace = 0.25f;
        // shoot the bullet
        while (canAttack && target != null)
        {
            // check if the target is in range
            if (Vector3.Distance(startTran.position, target.transform.position) > 10f) break;
            // check if the target is not null
            if (target == null) break;
            if (!canAttack) break;

            float zAngle = Mathf.Atan2(target.transform.position.y - transform.position.y, target.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            SpawnBullet(startTran, zAngle);
            yield return new WaitForSeconds(bulletSpace);

        }



    }
    private void SpawnBullet(Transform startTran, float zAngle)
    {
        if (bullet == null) return;

        GameObject bult = Instantiate(bullet, position: startTran.position, Quaternion.identity);
        bult.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, zAngle);
        bult.GetComponent<EnemyBullet>().SetBulletSpeedAndDamage(7f, damage);
    }

    void RotateSprite()
    {
        Vector2 targetPos = target.transform.position;
        // Transform of the cursor
        float angle = Mathf.Atan2(transform.position.y - targetPos.y, transform.position.x - targetPos.x);
        float correctAngle = angle * Mathf.Rad2Deg + 180;

        // set the cursor rotation
        transform.rotation = Quaternion.Euler(0, 0, correctAngle);

        // set the vertical flip of the sprite
        GunSprite.flipY = correctAngle > 90 && correctAngle < 270;
    }
}
