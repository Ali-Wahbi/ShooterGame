using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashEffect : MonoBehaviour
{
    [SerializeField] Animator anim;
    public float speed = 1.2f;
    private void Start()
    {
        anim.Play("SlashAnim");
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    public void OnAnimEnds()
    {
        Debug.Log("Slash anim ended");
        Destroy(gameObject);
    }

}
