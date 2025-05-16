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
    int spawnDropChance = 10;
    int weaponDropChance = 5;


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
                    // Debug.Log("Current State: " + currentState);
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
                    // Debug.Log("Current State: " + currentState);
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


    private void FixedUpdate()
    {

        Vector2 endPos = target ? target.transform.position : roamingPos;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, endPos, distance: 5);
        Debug.DrawRay(transform.position, endPos, Color.yellow);
        if (hit)
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "Breakable")
            {
                distance = 0;
            }
        }
    }


    void CalculateNewRoaming()
    {
        // Debug.Log("Set new Roaming");
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
            // Debug.Log("moveDirection changed.");
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

    private void SpawnDrops()
    {
        // spawn ammo
        int chance = Random.Range(0, 100);
        if (chance < spawnDropChance)
        {
            // spawn small ammo
            Debug.Log("Enemy Spawn ammo");
            Instantiate(GameAssets.g.AmmoPickUpPrefap, position: transform.position, rotation: Quaternion.identity);
            return;
        }
        if (chance < weaponDropChance)
        {
            // spawn a weapon
            Debug.Log("Enemy Spawn weapon");
            Instantiate(GameAssets.g.pickUpWeaponPrefap, position: transform.position.With(z: -0.1f), rotation: Quaternion.identity);
            return;
        }
    }


    public void MultDropChance(float amount)
    {
        spawnDropChance = (int)(spawnDropChance * amount);
        weaponDropChance = (int)(weaponDropChance * amount);
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
        // Debug.Log("Player entered detection area");
        SetAnimationBool(isAggressive, true);
        BotIsAggressive = true;
        target = other.gameObject;

    }

    public void OnDetectExitCollision()
    {
        // Debug.Log("Player exited detection area");
        // set time before cooldown
        if (currentState != EnemyStates.Defeated)
            StartCalmingDown();
    }

    int incRadius = 3;
    void StartCalmingDown()
    {
        if (BotIsDefeated) return;
        StartCoroutine(CalmingDown());

        IEnumerator CalmingDown()
        {
            yield return new WaitForSeconds(3f);
            SetAnimationBool(isAggressive, false);

            BotIsAggressive = false;
            target = null;

            if (isExpanded)
            {

                isExpanded = false;
                detectionArea.radius /= incRadius;

            }
        }
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
        currentState = EnemyStates.Defeated;
        SpawnDrops();
        HandleDissolve();
    }
    public float i = 0.01f;
    // handle dissolve disappear
    public void HandleDissolve()
    {
        // Material mat = GetComponent<SpriteRenderer>().material;
        // dissolve the enemy
        StartCoroutine(DissolveEnemy());

        IEnumerator DissolveEnemy()
        {
            yield return new WaitForSeconds(2f);
            // dissolve the enemy
            i = 0.01f;
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

    bool isExpanded = false;
    public void HandleHit()
    {
        if (!isExpanded)
        {
            isExpanded = true;
            detectionArea.radius *= incRadius;
        }


    }



}

public enum EnemyStates
{
    Idle, Walking, Attack, Defeated
}
