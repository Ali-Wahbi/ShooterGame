using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyerBot : MonoBehaviour
{
    // Actual name is Blastoid

    [SerializeField] int damage;
    [SerializeField] float originalSpeed;
    float movementSpeed;
    [SerializeField] CircleCollider2D detectionArea;
    Animator anim;
    SpriteRenderer sr;
    Rigidbody2D rb;
    Vector3 startPosition;
    public Vector3 roamingPos;
    public float distance;
    // public float AttackDistance;

    public EnemyStates currentState;
    EnemyStates prevState;
    public int moveDirection = 1;
    public int prevMoveDirection = 1;

    bool canWalk = true;
    bool BotIsAggressive = false;
    bool attackedPlayer = false;
    public GameObject target;

    bool BotIsDefeated = false;
    int spawnDropChance = 10;

    // enemy states as strings to be used with the animation controller
    [Space]
    [Header("Animations Names: ")]

    [SerializeField]
    string isWalking = "isWalking",
    isAggressive = "isAgitated",
    isDefeated = "isDefeated";


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        prevState = currentState = EnemyStates.Idle; // Initialize the current state to Idle
        startPosition = transform.position;
        movementSpeed = originalSpeed; // Set the initial movement speed to the original speed

        roamingPos = GetRoamingPosition();
        CheckFlip(pos: roamingPos);
    }

    // Update is called once per frame

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
            if (distance > 0)
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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, endPos, distance: 0.2f);
        Debug.DrawRay(transform.position, endPos, Color.yellow, 0.2f);
        if (hit)
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "Breakable")
            {
                Debug.Log("Blastoid hit a wall or breakable object, stopping movement.");
                distance = 0;
            }
        }
    }

    void AnimateMovement()
    {
        // If the state hasn't changed, no need to update animations
        if (prevState == currentState) return;

        switch (currentState)
        {
            case EnemyStates.Idle:
                SetAnimationBool(isWalking, false);
                prevState = EnemyStates.Idle;
                break;

            case EnemyStates.Walking:
                SetAnimationBool(isWalking, true);
                prevState = EnemyStates.Walking;
                break;

            case EnemyStates.Attack:
                SetAnimationBool(isAggressive, true);
                prevState = EnemyStates.Attack;
                break;

            case EnemyStates.Defeated:
                SetAnimationTrigger(isDefeated);
                prevState = EnemyStates.Defeated;
                break;

            default:
                break;
        }
    }
    Vector3 GetRoamingPosition()
    {
        Vector3 RandDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        float RandLength = Random.Range(2f, 5f);
        return startPosition + RandDir * RandLength;
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
        if (transform.position.x < pos.x) moveDirection = 1;

        if (transform.position.x > pos.x) moveDirection = -1;

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

    void SetAnimationBool(string boolName, bool boolValue)
    {
        anim.SetBool(boolName, boolValue);
    }

    void SetAnimationTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    public void HandleDefeat()
    {
        canWalk = false;
        attackedPlayer = false;

        // Disable box collider
        GetComponent<CircleCollider2D>().enabled = false;
        BotIsDefeated = true;
        currentState = EnemyStates.Defeated;
        // SpawnDrops();
        HandleDissolve();
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
    }

    float i = 0.001f;
    public void HandleDissolve()
    {
        StartCoroutine(DissolveEnemy());

        IEnumerator DissolveEnemy()
        {
            yield return new WaitForSeconds(2f);
            // the enemy disappears
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


    public void OnDetectEnterCollision(Collider2D other)
    {
        Debug.Log("Player entered -BlastOid- detection area");
        SetAnimationBool(isAggressive, true);
        BotIsAggressive = true;
        movementSpeed = originalSpeed * 1.5f; // Increase speed when aggressive
        target = other.gameObject;

    }


    public void OnDetectExitCollision()
    {
        // Debug.Log("Player exited detection area");
        // set time before cooldown
        // if (currentState != EnemyStates.Defeated)
        //     StartCalmingDown();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Blastoid collided with PlayerBody and is attacking!");
            // Attack the player
            if (!attackedPlayer)
            {
                collision.gameObject.GetComponent<PlayerStats>()?.TakeDamage(damage);
                attackedPlayer = true; // Prevent multiple attacks in one collision
            }
            AfterAttacking(); // Call AfterAttacking to reset state after attack
        }
    }
    void AfterAttacking()
    {
        if (canWalk)
        {
            canWalk = false;
            currentState = EnemyStates.Idle;

            movementSpeed = originalSpeed; // Reset movement speed to original

            StartCalmingDown();
            // Debug.Log("Current State: " + currentState);
        }
    }

    void StartCalmingDown()
    {
        StartCoroutine(CalmingDown());

        IEnumerator CalmingDown()
        {
            yield return new WaitForSeconds(2f);
            SetAnimationBool(isAggressive, false);

            BotIsAggressive = false;
            target = null;

            attackedPlayer = false; // Reset the attack state
        }
    }

}
