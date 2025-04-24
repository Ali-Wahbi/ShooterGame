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
    PowerButton currentSelectedPowerButton = null;
    HashSet<PowerButton> powerButtonsSet;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        powerButtonsSet = new HashSet<PowerButton>();
        // ShowPowerChoices();
    }
    PowerChamber chamber;
    public void ShowPowerChoices(PowerChamber _chamber)
    {
        chamber = _chamber;
        Info.SetActive(true);
        animator.SetTrigger("ShowInfo");

        StartCoroutine(AddPowerChoices());
    }

    IEnumerator AddPowerChoices()
    {
        // Clear existing power buttons
        foreach (Transform child in powerButtonHolder)
        {
            Destroy(child.gameObject);
        }
        powerButtonsSet.Clear();
        // Instantiate new power buttons
        for (int i = 0; i < numberOfPowers; i++)
        {
            // Instantiate a new power button
            GameObject powerButton = Instantiate(powerButtonPrefab, powerButtonHolder);
            PowerButton buttonScript = powerButton.GetComponent<PowerButton>();

            powerButtonsSet.Add(buttonScript);
            // Setup
            powerButton.name = "PowerButton_" + i;
            buttonScript.SetManager(this);
            yield return new WaitForSeconds(0.1f); // Optional delay for better visual effect
        }
    }

    bool TextShown = false;
    public async Task SetSelectedPower(string pName, string Pdesc, Vector2 position)
    {
        await SetSelectedPowerText(pName, Pdesc);
    }
    public async Task SetSelectedPower(PowerButton button)
    {
        UnSelectCurrentPowerButton();
        currentSelectedPowerButton = button;
        await SetSelectedPowerText(button.GetPowerName(), button.GetPowerDescription());
    }

    public void UnSelectCurrentPowerButton()
    {
        if (currentSelectedPowerButton != null)
        {
            currentSelectedPowerButton.UnSelectAnimation();
        }
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
    /// <summary>
    /// Formats the text to include color codes for specific words.
    /// </summary>
    /// <param name="textToFormat">the text string to return formatted</param>
    /// <returns>the string formatted to appear colorized</returns>
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

    /// <summary>
    /// Called when a power is selected from the UI.
    /// </summary>
    /// <param name="sprite"></param>
    public void OnSelectionMade(Sprite sprite)
    {
        onPowerChoicesFinished();

        // Hide the power selection UI
        PowerDisplayer dis = FindObjectOfType<PowerDisplayer>();


        PowerDisplayer.Pd.AddPowerSprite(sprite);
    }

    public void onPowerChoicesFinished()
    {

        HideAllPowerButtons();
        HideText();

        chamber.ChoicesFinished();
    }
    void HideAllPowerButtons()
    {
        foreach (PowerButton button in powerButtonsSet)
        {
            button.HideAnimation();
        }
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
