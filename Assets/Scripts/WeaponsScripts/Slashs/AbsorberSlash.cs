using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbsorberSlash : MonoBehaviour
{

    AbsorberSword swordParent;
    public void SetParent(AbsorberSword sParent)
    {
        swordParent = sParent;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (swordParent == null) return;

        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("Absorbing bullet");
            swordParent.OnSlashHitBullet();
            return;
        }
        else
        {
            swordParent.OnSlashMissedBullet();
            return;
        }
    }
}
