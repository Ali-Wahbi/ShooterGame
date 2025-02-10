using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthbarController : MonoBehaviour
{
    [Header("Bars Prefabs")]
    [SerializeField, Tooltip("Health Bar")] GameObject HpPrefap;
    [SerializeField, Tooltip("Shield Bar")] GameObject ShPrefap;
    [SerializeField, Tooltip("Empty Bar")] GameObject EBPrefap;

    [Header("Containers")]
    [SerializeField] RectTransform Background;
    [SerializeField] GameObject EmptyContainers;
    [SerializeField] GameObject FilledContainers;

    [SerializeField] float BackgroundPadding = 32;
    public int numberOfBars;


    public void IncMaxShield(int shieldCount)
    {
        // add child to the empty containers and one to the filled containers
        for (int i = 0; i < shieldCount; i++)
        {
            AddBGRightPadding();

            AddToEmptyContainer();
            AddToFilledContaier(ShPrefap);

        }

    }
    public void DecMaxShield(int shieldCount)
    {

        RemoveLastFromContainer(FilledContainers, shieldCount);
        RemoveLastFromContainer(EmptyContainers, shieldCount);
        // SubBGRightPadding();

    }

    public void IncShield(int shieldCount)
    {
        for (int i = 0; i < shieldCount; i++)
            AddToFilledContaier(ShPrefap);
    }
    public void DecFromShield(int shieldCount)
    {
        RemoveLastFromContainer(FilledContainers, shieldCount);

    }

    #region Background

    void SetBackgroundLength()
    {
        for (int i = 0; i < numberOfBars; i++)
        {
            AddBGRightPadding();
        }
    }
    void AddBGRightPadding()
    {
        Background.sizeDelta += new Vector2(BackgroundPadding, 0);
    }
    void SubBGRightPadding()
    {
        Background.sizeDelta -= new Vector2(BackgroundPadding, 0);
    }

    #endregion


    #region Foreground

    void AddToEmptyContainer()
    {
        GameObject bar = Instantiate(EBPrefap);
        bar.transform.SetParent(EmptyContainers.transform, false);
    }

    void AddToFilledContaier(GameObject prefap)
    {
        GameObject bar = Instantiate(prefap);
        bar.transform.SetParent(FilledContainers.transform, false);
    }
    public void AddFirstFilled()
    {
        AddToFilledContaier(HpPrefap);
        AddToEmptyContainer();
    }


    // for both the empts and filled containers
    void RemoveLastFromContainer(GameObject container, int count)
    {
        StartCoroutine(Remove(container, count));
    }
    IEnumerator Remove(GameObject container, int count)
    {
        for (int i = 0; i < count; i++)
        {

            int totalCount = container.transform.childCount;

            //TODO: Destroy but add animation later
            Destroy(container.transform.GetChild(totalCount - 1).gameObject);
            // Debug.Log("Remove Child from : " + container + "  At index : " + (totalCount - 1));
            yield return new WaitForSeconds(0.1f);

        }
    }

    #endregion

    // Testing area

    [ContextMenu("Add3Shield")]
    void Add1Shield()
    {
        IncShield(3);
    }
    [ContextMenu("Dec2Shield")]
    void Dec2Shield()
    {
        DecFromShield(2);
    }
}
