using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PauseWeaponDisplayer : MonoBehaviour
{

    [SerializeField] Image weapon1Image, weapon2Image;
    [SerializeField] RectTransform weapon1Rc, weapon2Rc;
    AttackingWeapon weapon1;
    AttackingWeapon weapon2;
    WeaponController weaponController;

    [Header("Text elements: ")]
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI weaponType;
    [SerializeField] TextMeshProUGUI weaponDamage;
    [SerializeField] TextMeshProUGUI weaponSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    void GetWeaponController()
    {
        weaponController = FindObjectOfType<WeaponController>();
        if (weaponController == null) Debug.LogError("Weapon Controller not found");
        else
        {
            weapon1 = weaponController.weapon1;
            weapon2 = weaponController.weapon2;
        }
    }

    void SetupSprites()
    {
        if (weapon1 == null) SetImageAlpha(weapon1Image, 0);
        else { weapon1Image.sprite = weapon1.GetWeaponSprite(); SetImageAlpha(weapon1Image, 1); }

        if (weapon2 == null) SetImageAlpha(weapon2Image, 0);
        else { weapon2Image.sprite = weapon2.GetWeaponSprite(); SetImageAlpha(weapon2Image, 1); }
    }
    void SetImageAlpha(Image image, float alpha)
    {
        image.color = image.color.WithAlpha(alpha);
    }

    public void ShowWeapons()
    {

        GetWeaponController();
        SetupSprites();

        TweenDisplay();
    }

    void TweenDisplay()
    {
        // weapon1Rc, weapon2Rc
        Vector2 startx = new Vector2(120f, 0f);
        Vector2 endx = Vector2.zero;

        weapon1Rc.anchoredPosition += startx;
        weapon2Rc.anchoredPosition += startx;

        weapon1Rc.DOAnchorPos(endx, 0.7f).SetDelay(0.2f)
        .SetUpdate(UpdateType.Normal, true)
        .SetEase(Ease.OutBounce);

        weapon2Rc.DOAnchorPos(endx, 0.7f).SetDelay(0.4f)
        .SetUpdate(UpdateType.Normal, true)
        .SetEase(Ease.OutBounce);


    }

    public void OnWeapon1Clicked()
    {
        if (weapon1 == null) return;
        OnWeaponClicked(weapon1);

    }

    public void OnWeapon2Clicked()
    {
        if (weapon2 == null) return;
        OnWeaponClicked(weapon2);
    }

    void OnWeaponClicked(AttackingWeapon weapon)
    {
        float delay = 0.15f;
        TweenWeaponText(weaponName, weapon.GetWeaponName(), delay * 0);

        string typeText;
        if (weapon.GetWeaponType() == WeaponType.Melee) typeText = "Melee";
        else typeText = "Ranged";

        TweenWeaponText(weaponType, typeText, delay * 1);
        TweenWeaponText(weaponDamage, "Damage: " + weapon.GetWeaponDamage() + " Ammo: " + weapon.GetWeaponAmmo(), delay * 1.3f);
        TweenWeaponText(weaponSpeed, "Speed: " + weapon.GetWeaponReloadSpeed(), delay * 1.7f);


    }
    void TweenWeaponText(TextMeshProUGUI textHolder, string endText = "", float delay = 0f)
    {
        textHolder.text = endText;
        float xOffset = -30;

        Vector2 endPos = textHolder.rectTransform.anchoredPosition;
        Vector2 startPos = textHolder.rectTransform.anchoredPosition.Add(x: xOffset);

        textHolder.rectTransform.anchoredPosition = startPos;
        textHolder.rectTransform.DOAnchorPos(endValue: endPos, 0.4f)
        .SetDelay(delay)
        .SetEase(Ease.OutQuad)
        .SetUpdate(UpdateType.Normal, true);

        DOTween.To(() => 0f, x => textHolder.color = textHolder.color.WithAlpha(x), 1f, 0.3f)
        .SetDelay(delay)
        .SetUpdate(UpdateType.Normal, true);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
