using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Rigidbody2D))]
public class BotMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] MovementSpeeds speedType;
    [SerializeField] float AttackDistace;
    float movementSpeed => GetMovementSpeed();

    [Header("Movement Speeds: ")]
    [SerializeField] float movementSpeedNormal = 0.007f;
    [SerializeField] float movementSpeedSlow = 0.003f;
    [SerializeField] float movementSpeedFast = 0.01f;
    [Space]
    [SerializeField] float distance, maxMoveDistance = 3;
    bool isWalking = false;
    bool canMove = true;
    public Vector3 moveDirection;

    private void Awake()
    {
        SetUpEvents();
    }
    // Start is called before the first frame update
    void Start()
    {
        SetMoveDirection();
        rb = GetComponent<Rigidbody2D>();
    }

    [Space]
    [SerializeField] float RaycastDistance = 1.5f;
    // Update is called once per frame
    void Update()
    {
        DetectRaycastCollision();
    }

    bool playerInRange = false;
    void FixedUpdate()
    {
        if (!canMove) return;
        if (!isWalking)
        {
            isWalking = true;
            if (moveDirection == Vector3.zero)
            {
                SetMoveDirection();
            }
        }
        rb.AddForce(moveDirection.normalized * Time.deltaTime * movementSpeed);

        if (target && distance <= AttackDistace)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                Debug.Log("Player in attack range.");
                OnPlayerWithinRange.Invoke();
            }
            moveDirection = GetNewRoamingPosition();
            distance += Time.deltaTime * 2;
        }
        else if (target && distance > AttackDistace)
        {
            if (playerInRange)
            {
                playerInRange = false;
                Debug.Log("Player out of attack range.");
                OnPlayerOutOfRange.Invoke();
            }
        }
        else if (distance <= maxMoveDistance)
        {
            distance += Time.deltaTime;
        }
        else
        {
            if (moveDirection != Vector3.zero)
                StopMoving();
        }
    }

    Transform target = null;

    void DetectRaycastCollision()
    {

        Vector2 endPos = target ? target.position : moveDirection.normalized;
        if (rb.velocity != Vector2.zero)
        {
            Debug.DrawRay(transform.position, moveDirection.normalized * RaycastDistance, Color.yellow);

            RaycastHit2D hit = Physics2D.Raycast(origin: transform.position, direction: endPos, distance: RaycastDistance);
            if (hit)
            {
                if (hit.transform.tag == "Wall" || hit.transform.tag == "Breakable")
                {
                    distance = maxMoveDistance;
                }
            }
        }
    }

    // called by the robot once the player is in the detection area 
    public void SetTarget(Transform t)
    {
        target = t;
    }

    [SerializeField] float waitBeforeMove = 0.5f;
    void StopMoving()
    {
        moveDirection = Vector3.zero;
        rb.velocity = new Vector2(0, 0);

        OnBotStopMoving.Invoke();
        StartCoroutine(WalkingStop());

        IEnumerator WalkingStop()
        {
            yield return new WaitForSeconds(waitBeforeMove);
            isWalking = false;

        }
    }
    // called by the robot once the player is in the detection area
    public void SetMoveDirection()
    {
        distance = 0;
        moveDirection = GetNewRoamingPosition();
        OnBotMoving.Invoke();
    }

    public void EndBotMovement()
    {
        moveDirection = Vector3.zero;
        rb.velocity = new Vector2(0, 0);
        canMove = false;
    }
    Vector3 GetNewRoamingPosition()
    {

        if (!isWalking) return Vector3.zero;


        Vector3 RandDir = new Vector3(Random.Range(-1, 1.1f), Random.Range(-1, 1.1f));
        float moveRange = Random.Range(1f, 3f);

        Vector3 finalDir = target ? (target.position - transform.position).normalized : RandDir;

        return finalDir * moveRange;
    }

    float GetMovementSpeed()
    {
        return speedType switch
        {
            MovementSpeeds.normal => movementSpeedNormal,
            MovementSpeeds.slow => movementSpeedSlow,
            MovementSpeeds.fast => movementSpeedFast,
            _ => movementSpeedNormal,
        };
    }
    #region Events

    UnityEvent OnPlayerWithinRange;
    UnityEvent OnPlayerOutOfRange;
    UnityEvent OnBotMoving;
    UnityEvent OnBotStopMoving;

    void SetUpEvents()
    {
        OnPlayerWithinRange = new UnityEvent();
        OnPlayerOutOfRange = new UnityEvent();
        OnBotMoving = new UnityEvent();
        OnBotStopMoving = new UnityEvent();
    }
    public void AddToPlayerInRange(UnityAction action) => OnPlayerWithinRange.AddListener(action);

    public void AddToPlayerOutOfRange(UnityAction action) => OnPlayerOutOfRange.AddListener(action);

    public void AddToBotMoving(UnityAction action) => OnBotMoving.AddListener(action);

    public void AddToBotStopMoving(UnityAction action) => OnBotStopMoving.AddListener(action);

    #endregion
}

enum MovementSpeeds
{
    normal, fast, slow
}
