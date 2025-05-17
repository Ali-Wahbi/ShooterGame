using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSwordSlash : MonoBehaviour
{
    GlassSword swordParent;

    public void SetParent(GlassSword sParent)
    {
        swordParent = sParent;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (swordParent == null) return;

        // Handle bullets
        if (other.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("shattering bullet with " + name);
            swordParent.OnSlashHitBullet(other.transform.position, transform.rotation);
            return;
        }
    }
}
