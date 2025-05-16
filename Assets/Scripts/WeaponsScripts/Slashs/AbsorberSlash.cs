using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbsorberSlash : MonoBehaviour
{

    public UnityEvent onBulletHitEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("Absorbing bullet");
            onBulletHitEvent.Invoke();
        }
    }
}
