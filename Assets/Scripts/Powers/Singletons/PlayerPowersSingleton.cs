using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowersSingleton : MonoBehaviour
{
    private static PlayerPowersSingleton _instance { get; set; }

    public static PlayerPowersSingleton Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<PlayerPowersSingleton>("PlayerPowersSingleton"));
            }
            return _instance;
        }
    }


    public List<PlayerPowersType> playerPowers = new List<PlayerPowersType>();

    public void UsePower(PlayerPowersType powerType)
    {
        playerPowers.Add(powerType);


        switch (powerType)
        {
            case PlayerPowersType.ExtraSAS:
                // Implement logic for ExtraSAS power
                Debug.Log("ExtraSAS power used");
                break;
            case PlayerPowersType.ExtraMagazine:
                // Implement logic for ExtraMagazine power
                Debug.Log("ExtraMagazine power used");
                break;
            default:
                Debug.LogWarning("Unknown player power type: " + powerType);
                break;
        }
    }

    private void OnEnable()
    {
        Debug.Log("PlayerPowersSingleton enabled");
    }

    private void OnDisable()
    {
        Debug.Log("PlayerPowersSingleton disabled");
    }
}
