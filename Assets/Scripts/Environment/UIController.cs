using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private HealthbarController healthbarController;
    [SerializeField] private AmmunitionUI ammoUIController;
    // Start is called before the first frame update

    public HealthbarController GetHealthbarController()
    {
        return healthbarController;
    }

    public AmmunitionUI GetAmmoUi()
    {
        return ammoUIController;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
