using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sprite;

    public void SetAnim(Vector2 directions, bool flip)
    {
        if (directions.x != 0 || directions.y != 0)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
        FlipSprite(flip);
    }
    void FlipSprite(bool flipX)
    {
        sprite.flipX = flipX;
    }

    public void SetIsDefeated()
    {
        anim.SetTrigger("Defeated");
    }
}
