using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTitleText : MonoBehaviour
{

    [SerializeField] private TitleText title;
    [SerializeField] private TitleText subTitle;

    [SerializeField] private string TitleText;
    [SerializeField] private string SubTitleText;


    // Start is called before the first frame update
    void Start()
    {
        if (title) title.SetUpText(TitleText);
        if (subTitle) subTitle.SetUpText(SubTitleText);
    }

}
