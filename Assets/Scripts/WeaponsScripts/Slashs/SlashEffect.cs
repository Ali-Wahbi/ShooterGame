using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [SerializeField] Animator anim;
    public float speed = 1.2f;
    int damage;
    public List<GameObject> HitedTargets = new List<GameObject>();
    private void Start()
    {
        anim.Play("SlashAnim");
    }

    public void Setup(int damage, float speed = 1.2f)
    {
        this.damage = damage;
        this.speed = speed;
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
