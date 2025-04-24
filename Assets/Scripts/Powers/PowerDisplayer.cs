using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject powerDisplayPrefab;
    [SerializeField] private GameObject powerDisplayContainer;


    private static PowerDisplayer _pd;

    public static PowerDisplayer Pd
    {
        get
        {
            if (_pd == null)
            {
                _pd = Instantiate(Resources.Load<PowerDisplayer>("Prefabs/Environment/Rooms/StaticRooms/PowersUI"));
            }
            return _pd;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneChangeManager.Scm.AddToRetryEvents(ResetDisplay);
        SceneChangeManager.Scm.AddToQuitEvents(ResetDisplay);
    }
    public HashSet<Sprite> PowerSprites;

    public void AddPowerSprite(Sprite sprite)
    {
        // add the sprite to save between levels
        if (PowerSprites == null) PowerSprites = new HashSet<Sprite>();
        PowerSprites.Add(sprite);


        GameObject child = new GameObject();

        child.name = sprite.name;
        child.transform.SetParent(powerDisplayContainer.transform);
        child.AddComponent<Image>();
        child.GetComponent<Image>().sprite = sprite;
    }


    public void ResetDisplay()
    {
        // remove all children from the display
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
