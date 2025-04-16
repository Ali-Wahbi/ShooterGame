using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : MonoBehaviour
{


    [SerializeField] Image imageHolder;
    private PowerChoicesManager manager;

    // static fields are used among all the instances of this class
    static PowerScriptableParent CurrentSelectedPower = null;
    static List<PowerScriptableParent> PowersChoices = new List<PowerScriptableParent>();
    static PowerScriptableParent[] LoadedPowers;

    public PowerScriptableParent SelectedPower;

    public void SetManager(PowerChoicesManager manager)
    {
        this.manager = manager;
    }
    public void SetImage(Sprite sprite)
    {
        imageHolder.sprite = sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (LoadedPowers == null) LoadAllPowers();
        while (!PowersChoices.Contains(SelectedPower))
        {
            PickRandomPower();
        }
        PowersChoices.Add(SelectedPower);
        SetImage(SelectedPower.power.PowerIcon);
    }

    void LoadAllPowers()
    {
        LoadedPowers = Resources.LoadAll<PowerScriptableParent>("Powers");

        if (LoadedPowers.Length == 0)
        {
            Debug.LogError("No powers found in Resources/Powers folder.");
        }
    }

    void PickRandomPower()
    {
        int randomIndex = Random.Range(0, LoadedPowers.Length);
        SelectedPower = LoadedPowers[randomIndex];
    }


    public void OnClick()
    {
        if (CurrentSelectedPower == SelectedPower)
        {
            AssignPower();
        }
        else
        {
            CurrentSelectedPower = SelectedPower;
            if (manager) manager.SetSelectedPowerText(SelectedPower.power.PowerDescription);
        }
    }


    void AssignPower()
    {
        if (SelectedPower is WeaponPower)
        {
            // PowerManager.Instance.SetSelectedPower(SelectedPower);
            // Debug.Log("Weapon Power Selected: " + SelectedPower.name);
            WeaponPowersSingleton.Instance.UsePower(((WeaponPower)SelectedPower).weaponPower);
        }
        else if (SelectedPower is PlayerPower)
        {
            // PowerManager.Instance.SetSelectedPower(SelectedPower);
            // Debug.Log("Player Power Selected: " + SelectedPower.name);

            PlayerPowersSingleton.Instance.UsePower(((PlayerPower)SelectedPower).playerPower);
        }
        else
        {
            Debug.Log("Power not found: " + SelectedPower.name);
        }
    }
}
