using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class PausePowerDisplayer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI title, description;

    HashSet<Power> AllPowers;
    // Start is called before the first frame update
    void Start()
    {
        // ShowPowers();
    }

    public void ShowPowers()
    {
        ClearChildren();
        // clear the text
        SetDisplayTexts("", "");
        AllPowers = PowerDisplayer.Pd.GetPowersSet();
        // foreach (Power pow in AllPowers)
        // {

        //     InstantiateSprite(pow);
        // }

        for (int i = 0; i < AllPowers.Count; i++)
        {
            Power pow = AllPowers.ToArray()[i];
            InstantiateSprite(pow, i);
        }

    }

    void InstantiateSprite(Power pow, float index = 0f)
    {
        GameObject holder = new GameObject(pow.PowerIcon.name + "_holder", typeof(RectTransform));
        holder.transform.SetParent(transform);

        GameObject child = new GameObject(pow.PowerIcon.name, typeof(RectTransform));
        child.transform.SetParent(holder.transform);

        child.AddComponent<Image>();
        child.GetComponent<Image>().sprite = pow.PowerOutlinedIcon;

        child.AddComponent<Button>();
        child.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(child.GetComponent<Image>()));
        TweenPowerSprite(child.GetComponent<Image>(), index);
    }

    public void OnButtonClick(Image image)
    {
        Debug.Log("Button Clicked with image: " + image.sprite.name);
        Power selectedPower = null;
        foreach (var power in AllPowers)
        {
            if (power.PowerOutlinedIcon.name == image.sprite.name)
            {
                selectedPower = power;
                break;
            }

        }
        if (selectedPower == null)
        {
            Debug.LogError("Power not found.");
            return;
        }

        ShowPowerText(selectedPower.PowerName, selectedPower.PowerDescription);
    }

    void ShowPowerText(string pName, string Pdesc)
    {
        // format the text based on the rules from the gameAssets object
        string descriptionText = GameAssets.g.FormatText(Pdesc);
        SetDisplayTexts(pName, descriptionText);


        TweenText(title, endText: pName);

        TweenText(description, endText: descriptionText, delay: 0.4f);


        // descriptionText
    }
    void SetDisplayTexts(string pName, string Pdesc)
    {
        title.text = pName;
        description.text = Pdesc;
    }

    void TweenPowerSprite(Image image, float index)
    {

        float startValue = 1f;
        float endValue = 1.2f;
        float delayValue = 0.2f;
        float delay = delayValue + delayValue * index;

        DOTween.To(() => startValue, y => image.rectTransform.localScale = new Vector3(y, y, y), endValue, 0.5f)
        .SetDelay(delay)
        .SetEase(Ease.OutBounce)
        .SetUpdate(UpdateType.Normal, true);

    }
    void TweenText(TextMeshProUGUI textHolder, string endText = "", float delay = 0f, bool tweenPos = true)
    {
        string startText = textHolder.text;
        DOTween.To(() => startText, x => textHolder.text = x, endText, 0f).SetDelay(delay).SetUpdate(UpdateType.Normal, true);

        CanvasGroup cg = textHolder.GetComponent<CanvasGroup>();

        DOTween.To(() => 0f, x => cg.alpha = x, 1f, 0.6f).SetDelay(delay).SetUpdate(UpdateType.Normal, true);

        if (!tweenPos) return;

        float descEndY = textHolder.rectTransform.anchoredPosition.y;
        float descStartY = textHolder.rectTransform.anchoredPosition.y + 15f;

        DOTween.To(() => descStartY, y => textHolder.rectTransform.anchoredPosition = new Vector2(textHolder.rectTransform.anchoredPosition.x, y), descEndY, 0.7f)
        .SetDelay(delay)
        .SetUpdate(UpdateType.Normal, true);


    }

    void ClearChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
