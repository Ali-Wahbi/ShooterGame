using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private Sprite BigDrop;
    [SerializeField] private Sprite SmallDrop;
    [SerializeField] private int BigAmmo;
    [SerializeField] private int SmallAmmo;

    [SerializeField, Range(1, 10)] private int BigAmmoChance = 3;
    [SerializeField, Range(0f, 0.5f), Tooltip("Wait before poping the second message")]
    float waitAmount;
    Transform target;
    public float movementSpeed;
    SpriteRenderer sp;

    // depend on the ammo drop size and the ammo type
    int ammoAmount;
    bool isCollected = false;

    float radius = 1f;

    float ammmoInc = 1f;

    int arowwAmmount
    {
        get
        {
            return (int)ammoAmount / 2;
        }
    }

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();

        SetRandomPick();
        SetAmmoAmount();
        SetRadius();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // set the ammo amount based on the sprite
        if (collision.CompareTag("Player"))
        {
            // get the player transform when entering the trigger
            target = collision.transform;
        }
    }


    void Update()
    {
        if (target)
        {
            float distance = Vector2.Distance(transform.position, target.position);

            movementSpeed += distance * 0.02f;

            // move towards the player
            transform.position = Vector2.MoveTowards(
                this.transform.position,
                target.transform.position,
                movementSpeed * Time.deltaTime);

            // if the distance is less than 0.1f, then the player has picked up the ammo
            if (distance < 0.1f)
            {
                if (!isCollected)
                {
                    HandleCollisionWithPlayer();
                    isCollected = true;
                }
            }

        }
    }

    void HandleCollisionWithPlayer()
    {
        target.GetComponent<PlayerStats>().AddAmmo(WeaponType.Bullets, ammoAmount);
        target.GetComponent<PlayerStats>().AddAmmo(WeaponType.Arrows, arowwAmmount);
        SendPopUp();
        sp.enabled = false;
    }


    /// <summary>
    /// Sets a random ammo pick-up sprite based on a random chance.
    /// </summary>
    /// <remarks>
    /// Generates a random number between 1 and 10. If the random number is less than or equal to the 
    /// specified BigAmmoChance, the sprite is set to BigDrop. Otherwise, the sprite is set to SmallDrop.
    /// </remarks>
    void SetRandomPick()
    {
        int random = Random.Range(1, 11);
        if (random <= BigAmmoChance)
        {
            sp.sprite = BigDrop;
        }
        else
        {
            sp.sprite = SmallDrop;
        }
    }

    /// <summary>
    /// Sets the amount of ammo based on the sprite of the ammo box.
    /// </summary>
    /// <remarks>
    /// If the sprite is BigDrop, the ammo amount is set to BigAmmo.
    /// If the sprite is SmallDrop, the ammo amount is set to SmallAmmo.
    /// Multiplied by ammmolnc.
    /// </remarks>
    private void SetAmmoAmount()
    {
        if (sp.sprite == BigDrop)
        {
            ammoAmount = BigAmmo * (int)ammmoInc;
        }
        else if (sp.sprite == SmallDrop)
        {
            ammoAmount = SmallAmmo * (int)ammmoInc;
        }
    }

    void SendPopUp()
    {
        StartCoroutine(ShowPopUp());

        IEnumerator ShowPopUp()
        {
            PopUpText.Create(displayText: $"+{ammoAmount} Bullets", pos: transform.position, color: PopupColor.Yellow, randomDirection: false);
            yield return new WaitForSeconds(waitAmount);

            PopUpText.Create(displayText: $"+{arowwAmmount} Arrows", pos: transform.position, color: PopupColor.Cyan, randomDirection: false);
            Destroy(gameObject);
        }
    }

    public void IncreasePickRadius()
    {
        radius *= 1.5f;
    }
    public void IncreaseAmmoAmount()
    {
        ammoAmount *= 2;
    }

    void SetRadius()
    {
        GetComponent<CircleCollider2D>().radius *= radius;
    }

}
