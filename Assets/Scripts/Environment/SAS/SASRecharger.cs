using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SASRecharger : MonoBehaviour
{

    public int NumberOfCharges = 3;
    public Sprite Charges_3;
    public Sprite Charges_2;
    public Sprite Charges_1;
    public Sprite Charges_0;

    public SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetSprite();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStats player = other.gameObject.GetComponent<PlayerStats>();

            int missingShield = player.GetMissingShield();

            if (missingShield > 0 && NumberOfCharges > 0)
            {
                Debug.Log("Player entered recharger, missing shield: " + missingShield);

                int remainder = Mathf.Max(0, NumberOfCharges - missingShield);
                player.RechargeShield(NumberOfCharges - remainder);
                NumberOfCharges = remainder;
                SetSprite();
            }
        }
    }

    void SetSprite()
    {
        switch (NumberOfCharges)
        {
            case 3:
                sr.sprite = Charges_3;
                break;

            case 2:
                sr.sprite = Charges_2;
                break;

            case 1:
                sr.sprite = Charges_1;
                break;

            case 0:
                sr.sprite = Charges_0;
                break;

            default:
                sr.sprite = Charges_3;
                break;
        }
    }
}
