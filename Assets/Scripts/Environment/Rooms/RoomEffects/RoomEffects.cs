using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;
public class RoomEffects : MonoBehaviour
{
    [SerializeField] private Curtain roomCurtain;
    [SerializeField] private CinemachineVirtualCamera roomCamera;
    [SerializeField] bool UseCurtain = true;
    int HighPrior = 10;
    int LowPrior = 0;
    bool BattleHasEnded = false;

    public CurtainDirection dire;

    void SetCameraPriority(int priority)
    {
        roomCamera.Priority = priority;
    }

    void SetCameraFollow(Transform target)
    {
        roomCamera.Follow = target;
    }
    // Start is called before the first frame update
    void Start()
    {
        roomCurtain.gameObject.SetActive(UseCurtain);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeField] List<EnemyStats> inRoomEnemies;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            if (BattleHasEnded) return;

            // get the player position relative to the room as up, down, left or right
            Vector2 playerPos = other.transform.position - transform.position;
            Debug.Log($"Player: {other.name} entered the room, position: {playerPos}");
            dire = GetPlayerDirection(playerPos);

            if (UseCurtain)
            {
                roomCurtain.SetCurtainDirection(direction: dire);
                roomCurtain.MakeCurtainDissapear();
            }

            SetCameraPriority(HighPrior);
            SetCameraFollow(other.transform);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            SetInRoomEnemies(enemy: other.GetComponent<EnemyStats>());
            // Debug.Log($"Enemy: {other.gameObject.name} entered the room");
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            CheckBattleEnded();
    }
    CurtainDirection GetPlayerDirection(Vector2 playerPos)
    {
        // up: y > 0, down: y < 0, left: x < 0, right: x > 0
        float standard = 10.0f;
        float offset = 2.0f;
        if (playerPos.y > standard && math.abs(playerPos.x) < offset)
        {
            Debug.Log("Player is up");
            return CurtainDirection.Up;
        }
        if (playerPos.y < standard && math.abs(playerPos.x) < offset)
        {
            Debug.Log("Player is down");
            return CurtainDirection.Down;
        }
        if (playerPos.x < standard && math.abs(playerPos.y) < offset)
        {
            Debug.Log("Player is left");
            return CurtainDirection.Left;
        }
        if (playerPos.x > standard && math.abs(playerPos.y) < offset)
        {
            Debug.Log("Player is right");
            return CurtainDirection.Right;
        }


        return CurtainDirection.Up;
    }


    void SetInRoomEnemies(EnemyStats enemy)
    {
        inRoomEnemies.Add(enemy);
        enemy.AddToOnDestroy(action: OnEnemyDestroied);
    }

    void OnEnemyDestroied()
    {
        // remove one enemy from the list
        inRoomEnemies.RemoveAt(0);
        CheckBattleEnded();
    }

    void CheckBattleEnded()
    {
        if (inRoomEnemies.Count == 0) onBattleEnd();
    }

    void onBattleEnd()
    {
        Debug.Log($"Battle in room {name} has ended");
        BattleHasEnded = true;
        SetCameraPriority(LowPrior);
    }

}
