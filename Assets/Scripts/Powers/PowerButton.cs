using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : MonoBehaviour
{


    [SerializeField] Image imageHolder;

    [SerializeField] Animator animator;
    private PowerChoicesManager manager;

    // static fields are used among all the instances of this class
    static PowerScriptableParent CurrentSelectedPower = null;
    static HashSet<PowerScriptableParent> LoadedPowers;

    public PowerScriptableParent SelectedPower;

    #region Setup
    // Start is called before the first frame update
    void Awake()
    {
        // Remove OR condition to avoid loading powers multiple times
        if (LoadedPowers == null || LoadedPowers.Count == 0) LoadAllPowers();
        PickRandomPower();
        SetImage();
    }

    void LoadAllPowers()
    {
        LoadedPowers = Resources.LoadAll<PowerScriptableParent>("Powers").ToHashSet();

        if (LoadedPowers.Count == 0)
        {
            Debug.LogError("No powers found in Resources/Powers folder.");
        }
    }

    void PickRandomPower()
    {
        int randomIndex = Random.Range(0, LoadedPowers.Count);
        SelectedPower = LoadedPowers.ElementAt(randomIndex);
        LoadedPowers.RemoveWhere(p => p == SelectedPower);

    }

    #endregion



    #region Utilities
    public void SetManager(PowerChoicesManager manager)
    {
        this.manager = manager;
    }
    void SetImage()
    {
        imageHolder.sprite = SelectedPower.power.PowerIcon;
    }

    public string GetPowerName()
    {
        return SelectedPower.power.PowerName;
    }
    public string GetPowerDescription()
    {
        return SelectedPower.power.PowerDescription;
    }
    #endregion
    #region Animations
    /// <summary>
    /// Selects the power button with an animation.
    /// </summary>
    void SelectAnimation()
    {
        string select = "Select";
        animator.SetTrigger(select);
    }
    // called by the parent manager when the power is unselected
    /// <summary>
    /// Unselects the power button with an animation.
    /// </summary>
    public void UnSelectAnimation()
    {
        string unSelect = "UnSelect";
        animator.SetTrigger(unSelect);
    }
    /// <summary>
    /// Hides the power button with an animation.
    /// </summary>
    public void HideAnimation()
    {
        GetComponentInChildren<CanvasGroup>().interactable = false;
        string hide = "Hide";
        animator.SetTrigger(hide);
    }
    #endregion
    public async void OnClick()
    {
        if (CurrentSelectedPower == SelectedPower)
        {
            // assign the power to the player
            AssignPower();
        }
        else
        {
            // select the power
            SelectAnimation();
            CurrentSelectedPower = SelectedPower;
            if (manager) await manager.SetSelectedPower(this);
        }
    }


    void AssignPower()
    {
        if (SelectedPower is WeaponPower)
        {
            WeaponPowersSingleton.Instance.UsePower(((WeaponPower)SelectedPower).weaponPower);
        }
        else if (SelectedPower is PlayerPower)
        {
            PlayerPowersSingleton.Instance.UsePower(((PlayerPower)SelectedPower).playerPower);
        }
        else
        {
            Debug.Log("Power not found: " + SelectedPower.name);
        }

        if (manager) manager.OnSelectionMade(SelectedPower.power);
        else Debug.LogError("PowerChoicesManager not assigned.");
    }
}
