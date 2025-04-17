using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class PowerChoicesManager : MonoBehaviour
{

    [SerializeField] GameObject powerButtonPrefab;
    [SerializeField] int numberOfPowers = 3;

    [Header("UI Elements")]
    [SerializeField] Transform powerButtonHolder;
    [SerializeField] GameObject Info;
    [SerializeField] TextMeshProUGUI selectedPowerNameText;
    [SerializeField] TextMeshProUGUI selectedPowerDescriptionText;
    [Header("Power Text Colors")]
    [SerializeField] Color SASColor = Color.cyan;
    [SerializeField] Color AmmoColor;
    [SerializeField] Color MeleeColor;
    [SerializeField] Color RangedColor;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ShowPowerChoices();
    }

    public void ShowPowerChoices()
    {
        Info.SetActive(true);
        animator.SetTrigger("ShowInfo");

        AddPowerChoices();
    }

    void AddPowerChoices()
    {
        // Clear existing power buttons
        foreach (Transform child in powerButtonHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfPowers; i++)
        {
            // Instantiate a new power button
            GameObject powerButton = Instantiate(powerButtonPrefab, powerButtonHolder);
            PowerButton buttonScript = powerButton.GetComponent<PowerButton>();

            // Setup
            powerButton.name = "PowerButton_" + i;
            buttonScript.SetManager(this);
        }
    }

    bool TextShown = false;
    public async Task SetSelectedPower(string pName, string Pdesc, Vector2 position)
    {
        await SetSelectedPowerText(pName, Pdesc);
    }
    public async Task SetSelectedPowerText(string pName, string Pdesc)
    {
        if (TextShown)
        {
            HideText();
            await Task.Delay(300);
            ShowText();
        }
        else
        {
            ShowText();
            TextShown = true;
        }
        // Set the selected power name and description text
        selectedPowerNameText.text = pName;
        selectedPowerDescriptionText.text = FormatText(Pdesc);

    }
    void ShowText()
    {
        animator.SetTrigger("Show");
    }
    void HideText()
    {
        animator.SetTrigger("Hide");
    }
    string FormatText(string textToFormat)
    {
        string SasColorHex = ColorUtility.ToHtmlStringRGBA(SASColor);
        string AmmoColorHex = ColorUtility.ToHtmlStringRGBA(AmmoColor);
        string MeleeColorHex = ColorUtility.ToHtmlStringRGBA(MeleeColor);
        string RangedColorHex = ColorUtility.ToHtmlStringRGBA(RangedColor);

        string formattedText = textToFormat.Replace("SAS", $"<color=#{SasColorHex}>SAS</color>")
                                            .Replace("Ammo", $"<color=#{AmmoColorHex}>Ammo</color>")
                                            .Replace("ammo", $"<color=#{AmmoColorHex}>ammo</color>")
                                            .Replace("Melee", $"<color=#{MeleeColorHex}>Melee</color>")
                                            .Replace("melee", $"<color=#{MeleeColorHex}>melee</color>")
                                            .Replace("ranged", $"<color=#{RangedColorHex}>ranged</color>")
                                            .Replace("Ranged", $"<color=#{RangedColorHex}>Ranged</color>");
        return formattedText;
    }


    public void onSelectionMade(Sprite sprite)
    {
        // Hide the power selection UI
        Debug.Log("Add child to power displayer");
        PowerDisplayer dis = FindObjectOfType<PowerDisplayer>();

        if (dis) dis.AddPowerSprite(sprite);
        else Debug.LogError("PowerDisplayer not found in the scene.");
    }

    private void OnEnable()
    {
        Cursor.visible = true;
    }
    private void OnDisable()
    {
        Cursor.visible = false;
    }

}
