using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CleanerBot : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float movementSpeed;
    [SerializeField] GameObject bullet;
    [SerializeField] CircleCollider2D detectionArea;
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
    bool canAttack = true;
    public GameObject target;

    bool BotIsDefeated = false;


    private void Start()
    {
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
        if (BotIsDefeated) return;

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
                    if (canAttack)
                        FireBullets(transform, target.transform.position);
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

    public void OnDetectEnterCollision(Collider2D other)
    {
        Debug.Log("Player entered detection area");
        SetAnimationBool(isAggressive, true);
        BotIsAggressive = true;
        target = other.gameObject;

    }

    public void OnDetectExitCollision()
    {
        Debug.Log("Player entered detection area");
        // set time before cooldown
        SetAnimationBool(isAggressive, false);
    }


    public void FireBullets(Transform startTran, Vector3 target)
    {
        StartCoroutine(ShootBullets(startTran, target));
    }


    IEnumerator ShootBullets(Transform startTran, Vector3 target)
    {
        float bulletSpace = 0.2f;
        float zAngle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
        // shoot the bullet
        SpawnBullet(startTran, zAngle);
        yield return new WaitForSeconds(bulletSpace);

        SpawnBullet(startTran, zAngle);
        yield return new WaitForSeconds(bulletSpace);

        SpawnBullet(startTran, zAngle);
        yield return new WaitForSeconds(bulletSpace);

        canAttack = false;

        // start cooldown timer
        AttackCooldown();

    }

    private void SpawnBullet(Transform startTran, float zAngle)
    {
        GameObject bult = Instantiate(bullet, position: startTran.position, Quaternion.identity);
        bult.GetComponent<Transform>().eulerAngles = new Vector3(0, 0, zAngle);
    }

    void AttackCooldown()
    {
        StartCoroutine(coolDown());

        IEnumerator coolDown()
        {
            yield return new WaitForSeconds(3);
            canAttack = true;
        }
    }

    public void HandleDefeat()
    {
        canWalk = false;
        canAttack = false;

        // Disable box collider
        GetComponent<BoxCollider2D>().enabled = false;
        BotIsDefeated = true;
        SetAnimationBool(isDefeated, true);
        Destroy(gameObject, 2.5f);
    }



}

public enum EnemyStates
{
    Idle, Walking, Attack, Defeated
}
