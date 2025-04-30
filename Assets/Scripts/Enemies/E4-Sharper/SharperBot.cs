using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SharperBot : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float movementSpeed;
    float originalSpeed;
    [SerializeField] CircleCollider2D detectionArea;
    [SerializeField] CircleCollider2D attackArea;
    [SerializeField] Animator anim;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] BotMovement botMovement;
    Vector3 startPosition;
    [Header("Testing")]
    public Vector3 roamingPos;
    public float distance;
    public float AttackDistance;
    // bullet

    // enemy states as strings to be used with the animation controller
    string isWalking = "IsWalking",
    isAggressive = "IsAggressive", isDefeated = "IsDefeated",

    // animation triggers
    Attack = "Attack", AttackEnd = "AttackEnd";


    public EnemyStates currentState;
    public EnemyStates prevState;
    public int moveDirection = 1;
    public int prevMoveDirection = 1;

    bool canWalk = true;
    bool BotIsAggressive = false;
    bool canAttack = true;
    public GameObject target;

    bool BotIsDefeated = false;
    // Start is called before the first frame update
    void Start()
    {
        originalSpeed = movementSpeed;
        startPosition = transform.position;

        roamingPos = GetRoamingPosition();

        CheckFlip();
        EventsSetUp();
        attackArea.GetComponent<CircleAttackArea>().SetDamage(damage);

    }

    // Update is called once per frame
    void Update()
    {
        AnimateMovement();
        if (BotIsDefeated) return;
        CheckFlip();
    }

    void AnimateMovement()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                SetAnimationBool(isWalking, false);
                break;

            case EnemyStates.Walking:
                SetAnimationBool(isWalking, true);
                break;

            case EnemyStates.Attack:
                SetAnimationBool(isWalking, true);
                movementSpeed = originalSpeed * 0.65f;
                SetAnimationBool(isAggressive, true);
                break;
            case EnemyStates.Defeated:
                SetAnimationBool(isDefeated, true);
                break;
            default:
                break;
        }
    }

    Vector3 GetRoamingPosition()
    {
        Vector3 RandPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        return startPosition + RandPos * Random.Range(2f, 5f);
    }

    void SetAnimationBool(string boolName, bool boolValue)
    {
        anim.SetBool(boolName, boolValue);
    }
    void SetAnimationTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }


    void CheckFlip(Vector3 pos)
    {
        if (transform.position.x < pos.x)
        {
            moveDirection = 1;
        }
        if (transform.position.x > pos.x)
        {
            moveDirection = -1;
        }
        if (prevMoveDirection != moveDirection)
        {
            bool flip = moveDirection == -1;
            // DoFlipBody(flip);
            // Debug.Log("moveDirection changed.");
            prevMoveDirection = moveDirection;
        }
    }


    void CheckFlip()
    {
        float velocityX = rb.velocity.x;

        if (velocityX > 0) moveDirection = 1;
        else if (velocityX < 0) moveDirection = -1;

        if (prevMoveDirection != moveDirection)
        {
            bool flip = moveDirection == -1;
            DoFlipBody(flip);
            // Debug.Log("moveDirection changed.");
            prevMoveDirection = moveDirection;
        }

    }


    void DoFlipBody(bool flip)
    {

        sr.flipX = flip;
        detectionArea.offset *= -1;

    }

    void EventsSetUp()
    {
        botMovement.AddToPlayerInRange(SetPlayerEnterRange);
        botMovement.AddToPlayerOutOfRange(SetPlayerExitRange);

        botMovement.AddToBotMoving(() => currentState = EnemyStates.Walking);
        botMovement.AddToBotStopMoving(() => currentState = EnemyStates.Idle);
    }

    void SetPlayerEnterRange()
    {
        SetAnimationTrigger(Attack);
        attackArea.enabled = true;
        currentState = EnemyStates.Attack;
    }

    void SetPlayerExitRange()
    {
        if (currentState != EnemyStates.Defeated)
        {
            StartCoroutine(StartCalmingDown());
        }

        IEnumerator StartCalmingDown()
        {
            yield return new WaitForSeconds(2f);
            SetAnimationBool(isAggressive, false);
            OnAttackEnd();
        }

    }

    public void OnDetectEnterCollision(Collider2D other)
    {
        // Debug.Log("Player entered detection area");
        SetAnimationBool(isAggressive, true);
        BotIsAggressive = true;
        target = other.gameObject;

        botMovement.SetTarget(t: other.transform);
        botMovement.SetMoveDirection();
    }

    public void OnDetectExitCollision()
    {
        // Debug.Log("Player exited detection area");
        // set time before cooldown
        if (currentState != EnemyStates.Defeated)
        {
            BotIsAggressive = false;
            botMovement.SetTarget(t: null);
            target = null;
        }
    }

    public void OnAttackEnd()
    {
        attackArea.enabled = false;
        canAttack = true;
        SetAnimationTrigger(AttackEnd);
        // Debug.Log("Sharper Attack end.");
    }
    public void onDefeated()
    {
        currentState = EnemyStates.Defeated;
        botMovement.EndBotMovement();
        attackArea.enabled = false;

        GetComponent<CapsuleCollider2D>().enabled = false;
        BotIsDefeated = true;



    }


}