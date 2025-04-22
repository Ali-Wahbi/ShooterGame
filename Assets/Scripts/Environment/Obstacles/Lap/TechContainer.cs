using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechContainer : MonoBehaviour
{
    [SerializeField] bool PlayTech1;
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        if (PlayTech1) animator.SetTrigger("Tech1");
        else animator.SetTrigger("Tech2");
    }
}
