using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveScript : MonoBehaviour
{
    float i;

    public void StartDissolve(SpriteRenderer sr, float WaitBefore = 2f)
    {
        // Material mat = GetComponent<SpriteRenderer>().material;
        // dissolve the enemy
        StartCoroutine(DissolveEnemy());

        IEnumerator DissolveEnemy()
        {
            yield return new WaitForSeconds(WaitBefore);
            // dissolve the enemy
            i = 0.01f;
            while (i < 1.1f)
            {
                i += Time.deltaTime * 2f;
                sr.material.SetFloat("_Fade", i);
                yield return null;
            }
            Debug.Log("Robot fully disappeared");
            // Destroy(gameObject);
        }
    }
}
