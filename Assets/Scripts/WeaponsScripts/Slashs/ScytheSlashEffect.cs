using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheSlashEffect : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Transform Projectile;
    public float speed = 1.2f;
    int damage;
    Transform projectileRotation;

    bool hittedTarget = false;
    public List<GameObject> HitedTargets = new List<GameObject>();
    private void Start()
    {
        // anim.Play("ScytheSlashAnim");
    }

    public void Setup(int damage, Transform rotation, float speed = 1.2f, bool flip = false)
    {
        this.damage = damage;

        projectileRotation = rotation;

        this.speed = speed;

        SetSpriteFlip(flip);

    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            hittedTarget = true;
        }
        if (other.gameObject.tag == "Enemy")
        {
            if (!HitedTargets.Contains(other.gameObject))
            {
                other.GetComponent<EnemyStats>().TakeDamage(damage);
                PopUpText.Create(damage: damage, pos: other.GetComponent<Transform>().position, color: PopupColor.White);
                HitedTargets.Add(other.gameObject);
                hittedTarget = true;
            }
        }
        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }

    }

    void SetSpriteFlip(bool filp)
    {
        GetComponent<SpriteRenderer>().flipY = filp;
    }

    public void OnAnimEnds()
    {
        // Debug.Log("Slash anim ended");
        HitedTargets.Clear();
        Destroy(gameObject);
        if (!hittedTarget)
        {
            //Summon scythe projectile
            Debug.Log("Summon scythe projectile.");
            SummonProjectile();
        }
    }

    void SummonProjectile()
    {
        Transform proj = Instantiate(Projectile, transform.position, projectileRotation.rotation);
        proj.GetComponent<ScytheProjectile>().Setup(damage: 4);
    }

}
