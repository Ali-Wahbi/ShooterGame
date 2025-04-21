using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerChamber : MonoBehaviour
{
    [SerializeField] PowerChoicesManager powerChoicesManager;

    //TODO: add the chamber Sprite and animations

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Cursor.visible = true;
            // Disable player movement and game pause here if needed
            // Show the power choices UI when the player enters the chamber
            powerChoicesManager.ShowPowerChoices(this);
        }
    }

    public void ChoicesFinished()
    {
        Cursor.visible = false;
    }

}
