using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanerBot : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int damage;
    [SerializeField] float movementSpeed;
    CircleCollider2D detectionArea;
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector3 startPosition;
    public Vector3 roamingPos;
    public float distance;
    public float AttackDistance;
    // bullet

    // enemy states as strings to be used with the animation controller
    string isWalking = "IsWalking";
    string isAggressive = "IsAggressive";
    string isDefeated = "IsDefeated";

    public EnemyStates currentState;
    public int moveDirection = 1;
    public int prevMoveDirection = 1;

    bool canWalk = true;
    bool BotIsAggressive = false;
    public GameObject target;


    private void Start()
    {
        detectionArea = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        roamingPos = GetRoamingPosition();
        CheckFlip(pos: roamingPos);

    }
    void Update()
    {
        AnimateMovement();

        Vector2 direction = roamingPos - transform.position;
        if (!BotIsAggressive)
        {
            if (canWalk)
            {
                distance = Vector2.Distance(transform.position, roamingPos);
                transform.position = Vector2.MoveTowards(this.transform.position, roamingPos, movementSpeed * Time.deltaTime);
            }
            if (distance == 0)
            {
                if (canWalk)
                {
                    canWalk = false;
                    currentState = EnemyStates.Idle;
                    Debug.Log("Current State: " + currentState);
                    roamingPos = GetRoamingPosition();
                    CalculateNewRoaming();
                }
            }
            else
            {
                currentState = EnemyStates.Walking;
                CheckFlip(pos: roamingPos);
            }
        }
        else
        {
            distance = Vector2.Distance(transform.position, target.transform.position);
            if (canWalk)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, target.transform.position, movementSpeed * Time.deltaTime);
            }
            if (distance <= AttackDistance)
            {
                if (canWalk)
                {
                    canWalk = false;
                    currentState = EnemyStates.Attack;
                    Debug.Log("Current State: " + currentState);
                    // Attack with 3 bullets
                }
            }
            else
            {
                canWalk = true;
                currentState = EnemyStates.Walking;
            }
            CheckFlip(pos: target.transform.position);

        }
    }

    void CalculateNewRoaming()
    {
        Debug.Log("Set new Roaming");
        if (currentState == EnemyStates.Idle)
        {
            StartCoroutine(SetNewRoam());

        }

        IEnumerator SetNewRoam()
        {
            yield return new WaitForSeconds(3);
            canWalk = true;
        }
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
            DoFlipBody(flip);
            Debug.Log("moveDirection changed.");
            prevMoveDirection = moveDirection;
        }
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
                SetAnimationBool(isWalking, false);
                SetAnimationBool(isAggressive, true);
                break;
            case EnemyStates.Defeated:
                SetAnimationBool(isDefeated, true);
                break;
            default:
                break;
        }
    }
    void SetAnimationBool(string boolName, bool boolValue)
    {
        anim.SetBool(boolName, boolValue);
    }




    Vector3 GetRoamingPosition()
    {
        Vector3 RandPos = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        return startPosition + RandPos * Random.Range(2f, 5f);
    }

    void DoFlipBody(bool flip)
    {

        sr.flipX = flip;
        detectionArea.offset *= -1;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered detection area");
            SetAnimationBool(isAggressive, true);
            BotIsAggressive = true;
            target = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entered detection area");
            SetAnimationBool(isAggressive, false);
        }
    }
}

public enum EnemyStates
{
    Idle, Walking, Attack, Defeated
}
