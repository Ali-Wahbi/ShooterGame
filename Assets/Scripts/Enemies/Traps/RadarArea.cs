using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RadarArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"Object {other.gameObject.name} entered Radar trigger area");
        GetComponentInParent<MosquitoTrap>().onBulletEntered(collider: other);

    }
}
