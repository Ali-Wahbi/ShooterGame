using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private HealthbarController Healthbar;

    // remove public later
    public int MaxSheild;

    public int CurrentShield;

    public int MissingShield = 0;


    // remove public later
    bool shieldIsBroken = false;
    bool shieldIsFull = false;
    // Start is called before the first frame update
    void Start()
    {
        // Set the UI start stats
        Healthbar.IncMaxShield(MaxSheild);
        Healthbar.DecFromShield(MissingShield);

        CurrentShield = MaxSheild - MissingShield;

        // set the shield start stat
        shieldIsBroken = CurrentShield == 0;
        shieldIsFull = CurrentShield == MaxSheild;
    }

    public void TakeDamage(int damage)
    {
        // Player dies if hit while shield is broken
        if (shieldIsBroken)
        {
            Debug.Log("Player Died");
            Healthbar.DecFromShield(1);
            return;
        }

        if (shieldIsFull) shieldIsFull = false;

        // Take Damage unitll 0 
        if (CurrentShield - damage <= 0)
        {

            Healthbar.DecFromShield(CurrentShield);

            CurrentShield = 0;
            MissingShield = MaxSheild;

            shieldIsBroken = true;
            return;
        }

        // Normal dec for the shield
        CurrentShield -= damage;
        SetMissingShield();
        Healthbar.DecFromShield(damage);
    }


    public void RechargeShield(int recharge)
    {
        // fill shield unitll max shield
        if (shieldIsFull) return;
        // fix broken shield
        if (shieldIsBroken) shieldIsBroken = false;

        // handle over charge
        if (CurrentShield + recharge >= MaxSheild)
        {
            Healthbar.IncShield(MaxSheild - CurrentShield);

            shieldIsFull = true;
            CurrentShield = MaxSheild;

            SetMissingShield();

            return;
        }

        // Normal recharge
        CurrentShield += recharge;
        SetMissingShield();
        Healthbar.IncShield(recharge);
    }

    // set the missing shield
    void SetMissingShield()
    {
        MissingShield = MaxSheild - CurrentShield;
    }

    public int GetMissingShield()
    {
        return MissingShield;
    }

    public void IncMaxShield(int addition)
    {
        MaxSheild += addition;
        // Handle UI
    }

    public void DecMaxShield(int subtraction)
    {
        MaxSheild -= subtraction;
        // Handle UI
    }

    // remove later
    [ContextMenu("Dec3Shield")]
    void DamagePlayer()
    {
        TakeDamage(3);
    }

    [ContextMenu("Inc3Shield")]
    void RechargePlayer()
    {
        RechargeShield(2);
    }
}
