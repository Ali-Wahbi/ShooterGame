using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Breakable : MonoBehaviour
{
    [Tooltip("Normal sprite to display")]
    [SerializeField] Sprite Normal;

    [Tooltip("Sprites of the object when it is broke. Pick randomly")]
    [SerializeField] List<Sprite> Broken;

    BoxCollider2D breakableCollider;
    SpriteRenderer SR;
    private void Reset()
    {
        gameObject.tag = "Breakable";
        SetCollider();
        SetRigidBody();
    }
    // Start is called before the first frame update
    void Start()
    {
        breakableCollider = GetComponent<BoxCollider2D>();
        SR = GetComponent<SpriteRenderer>();
        AtStart();
    }

    public void OnBreak()
    {
        DisableCollider();
        SetRandomSprite();
        // small exlposion then spawn drops
    }

    private void DisableCollider()
    {
        breakableCollider.enabled = false;
    }

    private void SetRandomSprite()
    {
        // Sprite rand = Random
        int randomChoice = Random.Range(0, Broken.Count);
        Sprite randomSprite = Broken[randomChoice];
        SR.sprite = randomSprite;
    }
    public void AtStart()
    {
        breakableCollider.enabled = true;
        SR.sprite = Normal;
    }

    #region Settings

    void SetRigidBody()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    void SetCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = false;
        collider.offset = new Vector2(0, -0.6f);
        collider.size = new Vector2(1, 0.7f);
    }


    #endregion
}
