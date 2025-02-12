using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMesh;
    Vector3 direction;
    float speed = 5f;
    float dissapearTimer = 1;
    Color originalColor;
    public void Setup(string textDamage, PopupColor color, bool randomDirection)
    {
        textMesh.text = "" + textDamage;
        SetTextColor(color);
        originalColor = textMesh.color;

        float min = -0.2f;
        float max = 0.2f;
        direction = new Vector2(Random.Range(min, max), Random.Range(min, max * 2.5f)).normalized;
        if (!randomDirection) direction = Vector2.up;
    }

    public static void Create(int damage, Vector3 pos, PopupColor color = PopupColor.Original, bool randomDirection = true)
    {
        Transform text = Instantiate(GameAssets.g.PopupPrefap, position: pos, rotation: Quaternion.identity);
        PopUpText popText = text.GetComponent<PopUpText>();
        Debug.Log("Create popup: " + popText);
        popText.Setup(textDamage: "" + damage, color: color, randomDirection: randomDirection);
    }

    public static void Create(string displayText, Vector3 pos, PopupColor color = PopupColor.Original, bool randomDirection = true)
    {
        Transform text = Instantiate(GameAssets.g.PopupPrefap, position: pos, rotation: Quaternion.identity);
        PopUpText popText = text.GetComponent<PopUpText>();
        Debug.Log("Create popup: " + popText);
        popText.Setup(textDamage: displayText, color: color, randomDirection: randomDirection);
    }



    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        speed -= Time.deltaTime * 0.85f * speed;

        dissapearTimer -= Time.deltaTime;

        if (dissapearTimer < 0)
        {
            originalColor.a -= 3f * Time.deltaTime;
            textMesh.color = originalColor;
            if (originalColor.a <= 0) Destroy(gameObject);
        }
    }

    void SetTextColor(PopupColor color)
    {
        switch (color)
        {
            case PopupColor.Original:
                break;
            case PopupColor.Cyan:
                textMesh.color = Color.cyan;
                break;
            case PopupColor.Red:
                textMesh.color = Color.red;
                break;
            case PopupColor.Yellow:
                textMesh.color = Color.yellow;
                break;
            case PopupColor.White:
                textMesh.color = Color.white;
                break;
            default:
                break;
        }
    }
}

public enum PopupColor
{
    Original, Cyan, Red, Yellow, White
}
