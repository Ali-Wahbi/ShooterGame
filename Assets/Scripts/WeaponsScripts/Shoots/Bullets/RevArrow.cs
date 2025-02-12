using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevArrow : Arrow
{
    // Rework the bullets system
    Vector2 endPos;
    public void SetArrowSpeedDamagePos(float speed, float damage, Vector2 startPos)
    {
        SetBulletSpeedAndDamage(speed, damage);
        endPos = startPos;
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
        float distance = Vector2.Distance(transform.position, endPos);
        if (distance <= 0.7f)
        {
            base.DestroyArrow();
        }
    }
}
