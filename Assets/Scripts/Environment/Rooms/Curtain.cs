using System.Collections;
using UnityEngine;

/// <summary>
/// Used to make the curtain of the room appear and dissapear
/// </summary>
public class Curtain : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    float currentCurtainValue = 0f;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;
        SetMaterialValue(0f);
    }
    // set by parent room 
    /// <summary>
    /// Set the direction of the curtain to dissapear from
    /// </summary>
    /// <param name="direction"> from the Enum: CurtainDirection</param>
    public void SetCurtainDirection(CurtainDirection direction)
    {
        int Up = 0, Right = 0, Down = 0, Left = 0;
        switch (direction)
        {
            case CurtainDirection.Up:
                Up = 1;
                break;
            case CurtainDirection.Right:
                Right = 1;
                break;
            case CurtainDirection.Down:
                Down = 1;
                break;
            case CurtainDirection.Left:
                Left = 1;
                break;
            default:
                break;
        }
        Vector4 directions = new Vector4(Up, Right, Down, Left);
        SetMaterialVector(directions);
    }

    // called by parent room
    /// <summary>
    /// Make the curtain dissapear in 0.7 seconds
    /// </summary>
    /// <param name="time">in seconds, 0.7s by default</param>
    public void MakeCurtainDissapear(float time = 0.7f, float toValue = 1.0f)
    {
        float originalTime = time;
        StartCoroutine(DissapearCoroutine());
        // make the curtain dissapear in 0.7 seconds
        IEnumerator DissapearCoroutine()
        {
            float value = currentCurtainValue;
            while (time > 0 && value <= toValue)
            {
                time -= Time.deltaTime;
                value += Time.deltaTime / originalTime;
                SetMaterialValue(value);

                yield return null;
            }
            SetMaterialValue(toValue);
        }
    }


    // called by parent room

    /// <summary>
    /// Make the curtain appear in 0.7 seconds
    /// </summary>
    /// <param name="time">in seconds, 0.7s by default</param>
    public void MakeCurtainAppear(float time = 0.7f, float toValue = 0.0f)
    {
        // TODO: Use Tween instead of Coroutine
        float originalTime = time;
        StartCoroutine(AppearCoroutine());

        // make the curtain dissapear in 0.7 seconds
        IEnumerator AppearCoroutine()
        {
            spriteRenderer.enabled = true;

            float value = currentCurtainValue;
            while (time > 0 && value >= toValue)
            {
                time -= Time.deltaTime;
                value -= Time.deltaTime / originalTime;
                SetMaterialValue(value);
                yield return null;
            }
            SetMaterialValue(toValue);
        }
    }

    public void MakeCurtainPartiallyDissapear()
    {
        float toValue = 0.3f;
        MakeCurtainDissapear(time: 0.3f, toValue: toValue);
    }

    public void MakeCurtainPartiallyAppear()
    {
        MakeCurtainAppear(time: 0.3f, toValue: 0.0f);
    }

    /// <summary>
    /// Set the material vector to the direction of the curtain
    /// </summary>
    /// <param name="dir">the direction of the player given in Vector 4 form</param>
    private void SetMaterialVector(Vector4 dir)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetVector("_Directions", dir);
    }

    /// <summary>
    /// Set the material value to the value of the curtain
    /// </summary>
    /// <param name="value"></param>
    private void SetMaterialValue(float value)
    {
        currentCurtainValue = value;
        spriteRenderer.material.SetFloat("_Value", value);
    }

}

public enum CurtainDirection
{
    Up, Right, Down, Left,
}