using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplayer : MonoBehaviour
{

    public HashSet<Sprite> PowerSprites;

    public void AddPowerSprite(Sprite sprite)
    {
        // add the sprite to save between levels
        if (PowerSprites == null) PowerSprites = new HashSet<Sprite>();
        PowerSprites.Add(sprite);


        GameObject child = new GameObject();

        child.transform.SetParent(this.transform);
        child.AddComponent<Image>();
        child.GetComponent<Image>().sprite = sprite;
    }
}
