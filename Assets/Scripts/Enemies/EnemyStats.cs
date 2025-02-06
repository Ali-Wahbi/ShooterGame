using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{

    [SerializeField] int health;
    [Header("Flash Hit: ")]
    [SerializeField] bool UseFlash = true;
    [SerializeField, Range(0, 1)] float FlashDuration;
    [SerializeField] Material FlashMaterial;
    private Material OriginalMaterial;
    private Coroutine FlashCoroutine;
    private SpriteRenderer sp;
    [Header("Events: ")]
    [SerializeField] UnityEvent DefeatEvents;
    [SerializeField] UnityEvent HittedEvents;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        OriginalMaterial = sp.material;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0) HittedEvents.Invoke();

        // use flash effect on hit
        if (UseFlash) StartFlash();

        Debug.Log(name + " took damage, health: " + health);

        if (health <= 0) DefeatEvents.Invoke();

    }

    void StartFlash()
    {
        // Stop the Coroutine if it is started
        if (FlashCoroutine != null)
        {
            StopCoroutine(FlashCoroutine);
        }

        FlashCoroutine = StartCoroutine(FlashEffect());

    }

    private IEnumerator FlashEffect()
    {
        sp.material = FlashMaterial;

        yield return new WaitForSeconds(FlashDuration);

        sp.material = OriginalMaterial;

        FlashCoroutine = null;
    }
}
