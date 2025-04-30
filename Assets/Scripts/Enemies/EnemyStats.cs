using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// [RequireComponent(typeof(BotMovement))]
public class EnemyStats : MonoBehaviour
{

    [SerializeField] int health;
    [Header("Flash Hit: ")]
    [SerializeField] bool UseFlash = true;
    [SerializeField, Range(0, 1)] float FlashDuration;
    [SerializeField] Material FlashMaterial;
    [SerializeField] bool UseDissolve = true;
    private Material OriginalMaterial;
    private Coroutine FlashCoroutine;
    private SpriteRenderer sp;
    [Header("Events: ")]
    [SerializeField] UnityEvent DefeatEvents;
    [SerializeField] UnityEvent HittedEvents;

    private void Reset()
    {
        gameObject.tag = "Enemy";
    }

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

        if (health <= 0) OnEnemyDefeat();

    }

    void OnEnemyDefeat()
    {
        DefeatEvents.Invoke();

        if (UseDissolve) HandleDissolve();
        HandleRechargeableBatteriesPower();
    }

    void HandleRechargeableBatteriesPower()
    {
        bool canRecharge = PlayerPowersSingleton.Instance.RechargableBatteriesActive;

        float chance = Random.Range(0f, 100f);
        float chanceToRecharge = 14f; // should be 14% chance to recharge
        if (canRecharge && chance <= chanceToRecharge)
        {
            PlayerPowersSingleton.Instance.OnRechargeBatteriesCalled(transform.position);
        }
    }

    public void AddToOnDestroy(UnityAction action)
    {
        DefeatEvents.AddListener(action);
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

    public void HandleDissolve()
    {
        // Material mat = GetComponent<SpriteRenderer>().material;
        // dissolve the enemy
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        StartCoroutine(DissolveEnemy());

        IEnumerator DissolveEnemy()
        {
            yield return new WaitForSeconds(2f);
            // dissolve the enemy
            float i = 0.01f;
            while (i < 1.1f)
            {
                i += Time.deltaTime * 2f;
                sr.material.SetFloat("_Fade", i);
                yield return null;
            }
            Debug.Log("Robot fully disappeared");
            Destroy(gameObject);
        }
    }
}
