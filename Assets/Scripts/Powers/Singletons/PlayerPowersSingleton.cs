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

    PlayerStats playerStats;

    public List<PlayerPowersType> playerPowers = new List<PlayerPowersType>();


    [Header("Player Powers")]

    /// <summary>
    /// <b>Extra SAS power:</b> increases the shield by 4 points
    /// </summary>
    public int ShieldExtra = 0;

    /// <summary>
    /// <b>Extra Magazine power:</b> increases the magazine size from 1 to 1.5f
    /// </summary>
    public float MaxMagazineSizeMultiplier = 1f;

    /// <summary>
    /// <b>Binoculars power:</b> increases the camera zoom from 6f to 7f
    /// </summary>
    public float DefaultCameraZoom = 6f;

    /// <summary>
    /// <b>Enhanced sprinters power:</b> increases the player speed from 1 to 1.3f
    /// </summary>
    public float PlayerSpeedMultiplier = 1f;

    public bool RechargableBatteriesActive = false;

    public void UsePower(PlayerPowersType powerType)
    {
        if (playerStats == null)
        {
            playerStats = FindAnyObjectByType<PlayerStats>();
        }
        playerPowers.Add(powerType);


        switch (powerType)
        {
            case PlayerPowersType.SASExtension:
                ShieldExtra = 4;
                playerStats?.SetMaxShield();
                break;

            case PlayerPowersType.ExtraMagazine:
                MaxMagazineSizeMultiplier = 1.5f;
                break;

            case PlayerPowersType.Binoculars:
                DefaultCameraZoom = 7f;
                playerStats.SetCameraZoom();
                break;

            case PlayerPowersType.EnhancedSprinters:
                PlayerSpeedMultiplier = 1.3f;
                break;

            case PlayerPowersType.RechargableBatteries:
                RechargableBatteriesActive = true;
                break;

            default:
                Debug.LogWarning("Unknown player power type: " + powerType);
                break;

        }
    }


    public void ResetPowers()
    {
        playerPowers.Clear();
        ShieldExtra = 0;
        MaxMagazineSizeMultiplier = 1f;
        DefaultCameraZoom = 6f;
        PlayerSpeedMultiplier = 1f;
        RechargableBatteriesActive = false;
    }

    public void OnRechargeBatteriesCalled(Vector3 pos)
    {
        int RechargeAmount = 2; // Amount to recharge

        playerStats.RechargeShield(RechargeAmount);
        PopUpText.Create(displayText: $"+{RechargeAmount} charges", pos: pos, color: PopupColor.Cyan, randomDirection: false);
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
