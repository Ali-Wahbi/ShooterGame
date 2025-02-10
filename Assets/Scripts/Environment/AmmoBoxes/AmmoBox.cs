using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{

    [SerializeField] private int ammoBoxesAmount = 3;

    Animator anim;

    private Transform AmmoPickups;

    private void Start()
    {
        anim = GetComponent<Animator>();
        AmmoPickups = GameAssets.g.AmmoPickUpPrefap.transform;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // spawn ammo for the player
            SpawnAmmoPickups();
            // animate the ammo box
            anim.SetTrigger("Open");

            // disable the collider
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void SpawnAmmoPickups()
    {
        StartCoroutine(SpawnAmmo());

        IEnumerator SpawnAmmo()
        {

            for (int i = 0; i <= ammoBoxesAmount; i++)
            {
                Instantiate(AmmoPickups, position: transform.position, rotation: Quaternion.identity);
                yield return new WaitForSeconds(0.25f);
            }
        }
    }

    public void IncAmmoBoxesAmount(int amount = 1)
    {
        ammoBoxesAmount += amount;
    }
}
