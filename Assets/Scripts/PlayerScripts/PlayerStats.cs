using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private HealthbarController Healthbar;

    public int MaxSheild;

    public int CurrentShield;

    public int MissingShield = 0;


    // Start is called before the first frame update
    void Start()
    {
        Healthbar.IncMaxShield(MaxSheild);
        Healthbar.DecFromShield(MissingShield);
        CurrentShield = MaxSheild - MissingShield;
    }

    void TakeDamage(int damage)
    {
        // Take Damage unitll 0 
        CurrentShield -= damage;
        Healthbar.DecFromShield(damage);
    }

    void RechargeShield(int recharge)
    {
        // fill shield unitll max shield
        CurrentShield -= recharge;
        Healthbar.IncShield(recharge);
    }
}
