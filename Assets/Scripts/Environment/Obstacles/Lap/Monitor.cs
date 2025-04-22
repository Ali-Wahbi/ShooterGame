using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : MonoBehaviour
{
    [SerializeField] private List<Sprite> monitorSprites;
    [SerializeField] private Sprite EmptySprite;

    SpriteRenderer monitorSpriteRenderer;
    float timer
    {
        get
        {
            return Random.Range(2f, 4.5f);
        }
    }
    float EmptySpriteTimer = 0.3f;


    int previousMonitorNumber = -1;
    int monitorNumber
    {
        get
        {
            int randomNumber = Random.Range(0, monitorSprites.Count);
            if (randomNumber == previousMonitorNumber)
            {
                return monitorNumber;

            }
            else
            {
                previousMonitorNumber = randomNumber;
                return randomNumber;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        monitorSpriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(MonitorAnimation());
    }

    IEnumerator MonitorAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(EmptySpriteTimer);
            SetSprite(monitorSprites[monitorNumber]);
            yield return new WaitForSeconds(timer);
            SetSprite(EmptySprite);
        }
    }

    void SetSprite(Sprite sprite)
    {
        monitorSpriteRenderer.sprite = sprite;
    }
}
