using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : MonoBehaviour
{
    [SerializeField] private GameObject alarmLight;
    SpriteRenderer AlarmLightSpriteRenderer;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField, Range(0f, 1f)] float minVisiblity = 0.7f;
    // Start is called before the first frame update
    void Start()
    {
        AlarmLightSpriteRenderer = alarmLight.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame to rotate the alarm light and change its visibility
    void Update()
    {
        alarmLight.transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (AlarmLightSpriteRenderer.color.a > minVisiblity)
        {
            Color color = AlarmLightSpriteRenderer.color;
            color.a -= Time.deltaTime * 0.5f;
            AlarmLightSpriteRenderer.color = color;
        }
        else
        {
            Color color = AlarmLightSpriteRenderer.color;
            color.a = 1f;
            AlarmLightSpriteRenderer.color = color;
        }
    }
}
