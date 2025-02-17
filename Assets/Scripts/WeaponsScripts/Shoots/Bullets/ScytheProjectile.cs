using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheProjectile : MonoBehaviour
{

    [SerializeField] float speed = 1.2f;
    [SerializeField] Transform spriteChild;
    int damage;


    public void Setup(int damage)
    {
        this.damage = damage;

    }

    public float RoateSpeed = -3;
    public float ReduceSpeedOffset = 0.5f;

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        spriteChild.Rotate(0, 0, RoateSpeed);

        if (speed > 0) speed -= Time.deltaTime * ReduceSpeedOffset;
        if (speed <= 0) Destroy(gameObject, 1);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Breakable")
        {
            other.gameObject.GetComponent<Breakable>().OnBreak();
        }
        if (other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyStats>().TakeDamage(damage);
            PopUpText.Create(damage: damage, pos: other.GetComponent<Transform>().position, color: PopupColor.White);


        }
        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            Destroy(other.gameObject);
        }

    }

}
