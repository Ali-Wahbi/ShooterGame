using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] float speed = 1.2f;
    int damage;
    public List<GameObject> HitedTargets = new List<GameObject>();

    float SlashSpeedMultiplierPower
    {
        get
        {
            return WeaponPowersSingleton.Instance.SlashSpeedMultiplierPower;
        }
    }

    bool canReflectBullet
    {
        get
        {
            return WeaponPowersSingleton.Instance.CanReflect;
        }
    }


    private void Start()
    {
        anim.Play("SlashAnim");
    }

    public void Setup(int damage)
    {
        this.damage = damage;
        // this.speed = speed;
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed * SlashSpeedMultiplierPower;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
            Debug.Log("Hited Breakable on trigger ");
        }
        if (other.gameObject.tag == "Enemy")
        {
            if (!HitedTargets.Contains(other.gameObject))
            {
                other.GetComponent<EnemyStats>().TakeDamage(damage);
                PopUpText.Create(damage: damage, pos: other.GetComponent<Transform>().position, color: PopupColor.White);
                HitedTargets.Add(other.gameObject);
            }
        }
        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            if (canReflectBullet)
            {
                other.gameObject.GetComponent<EnemyBullet>()?.ReflectBullet(transform.eulerAngles.z);
            }
            Destroy(other.gameObject);
        }

    }


    public void OnAnimEnds()
    {
        // Debug.Log("Slash anim ended");
        HitedTargets.Clear();
        Destroy(gameObject);
    }

}
