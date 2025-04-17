using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PowerButton : MonoBehaviour
{


    [SerializeField] Image imageHolder;
    private PowerChoicesManager manager;

    // static fields are used among all the instances of this class
    static PowerScriptableParent CurrentSelectedPower = null;
    static List<PowerScriptableParent> PowersChoices = new List<PowerScriptableParent>();
    static HashSet<PowerScriptableParent> LoadedPowers;

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
        PickRandomPower();
        PowersChoices.Add(SelectedPower);
        SetImage(SelectedPower.power.PowerIcon);
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


    public async void OnClick()
    {
        if (CurrentSelectedPower == SelectedPower)
        {
            AssignPower();
        }
        else
        {
            CurrentSelectedPower = SelectedPower;
            if (manager) await manager.SetSelectedPower(
                pName: SelectedPower.power.PowerName,
                Pdesc: SelectedPower.power.PowerDescription,
                position: transform.position);
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

        if (manager) manager.onSelectionMade(SelectedPower.power.PowerOutlinedIcon);
        else Debug.LogError("PowerChoicesManager not assigned.");
    }
}
