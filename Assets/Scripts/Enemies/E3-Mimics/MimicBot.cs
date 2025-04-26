using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MimicBot : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int damage;

    // will be used to switch between the 2 mimic bots
    [SerializeField] bool isSASBot = true;
    [SerializeField] MimicWeapon weapon;
    [SerializeField] CircleCollider2D detectCollider;
    int botNumber = 1;
    bool isAggressive = false, hasAppeared = false;

    [SerializeField] mimicStates currentState = mimicStates.Idle;
    [SerializeField] mimicStates prevState = mimicStates.Appear;

    float LookingTimer
    {
        get
        {
            return Random.Range(5f, 10f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isSASBot) botNumber = 2;

        weapon.SetDamage(damage);
        weapon.SetCanAttack(false);
    }

    // Update is called once per frame
    void Update()
    {
        Animations();
    }

    void Animations()
    {
        if (prevState == currentState) return;

        switch (currentState)
        {
            case mimicStates.Idle:
                SetAnimationTrigger(idle);
                _ = SetLookingTimer();
                break;
            case mimicStates.Looking:
                SetAnimationTrigger(looking);
                break;
            case mimicStates.Appear:
                SetAnimationTrigger(appear);
                break;
            case mimicStates.Defeated:
                SetAnimationTrigger(defeated);
                break;
        }
        prevState = currentState;
    }
    string idle = "Idle", looking = "Looking", appear = "Appear", defeated = "Defeated";

    void SetAnimationTrigger(string triggerName)
    {
        string trigger = triggerName + botNumber.ToString();
        animator.SetTrigger(trigger);
    }

    async Task SetLookingTimer()
    {
        float timer = LookingTimer;
        // Debug.Log("Looking Timer: " + timer);
        await Task.Delay((int)(timer * 1000));
        if (!hasAppeared) currentState = mimicStates.Looking;
    }

    public void OnDetectEnterCollision(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!hasAppeared) SetAnimationTrigger(appear);
        else weapon.SetCanAttack(true);
        isAggressive = true;
        // attack player
        weapon.SetTarget(other.gameObject);
    }

    public void OnDetectExitCollision()
    {
        if (isAggressive) isAggressive = false;
        // stop attacking
        weapon.SetCanAttack(false);
    }

    public void MimicAppeared()
    {
        hasAppeared = true;
        // attack the player
        weapon.SetCanAttack(true);
    }
    public void MimicLookingEnded()
    {
        // stop looking
        currentState = mimicStates.Idle;
    }

    public void MimicDefeated()
    {
        // stop attacking
        weapon.SetCanAttack(false);
        detectCollider.enabled = false;
        currentState = mimicStates.Defeated;

    }
    public void MimicDissolve()
    {
        // dissolve the enemy
        GetComponent<DissolveScript>().StartDissolve(GetComponent<SpriteRenderer>(), 0.7f);

    }
}

enum mimicStates
{
    Idle, Looking, Appear, Defeated
}