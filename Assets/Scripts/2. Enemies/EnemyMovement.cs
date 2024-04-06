using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))] // Ensure there's a Rigid body 2D component attached to the GameObject.
public class EnemyMovement : MonoBehaviour
{
    [Header("Component Connections")]
    //[SerializeField] private Vector3Variable playerPosition;
    private Rigidbody2D _rb;
    private Vector2 _movementDirection;
    [SerializeField] private float minDistanceToPlayer = 0.2f;
    [SerializeField] private EnemyStatsController enemyStatsController;
    [SerializeField] private PlayerStatsController[] playerStatsControllers;

    [SerializeField] private int runToDistance = 25;
    private float _originalMoveSpeed;
    [SerializeField] private float runSpeedMultiplier = 1f; // 1 = normal speed, 2 = double speed



    private void Awake()
    {
        var players = GameManager.GetPlayerGameObjects();
        playerStatsControllers = new PlayerStatsController[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            playerStatsControllers[i] = players[i].GetComponent<PlayerStatsController>();
        }
        
        _rb = GetComponent<Rigidbody2D>();
        
        _originalMoveSpeed = enemyStatsController.GetMoveSpeed();

    }

    private void FixedUpdate()
    {
        if (enemyStatsController.GetIsFoundByPlayer() == false)
        {
            CheckIfPlayerIsClose();
            return;
        }
            
        UpdateMovementDirection();
        MoveEnemy();
    }
    
    private void CheckIfPlayerIsClose()
    {
        foreach (var playerStatsController in playerStatsControllers)
        {
            if (Vector2.Distance(transform.position, playerStatsController.GetPlayerPosition()) < 20f)
            {
                enemyStatsController.SetIsFoundByPlayer(true);
                GetComponent<EnemyTeleportToPlayer>().enabled = true;
            }
        }
    }

    // Enemies will move towards the nearest player
    private void UpdateMovementDirection()
    {
        GameObject nearestPlayer = GameManager.GetNearestPlayer(transform.position);
        if (nearestPlayer != null)
        {
            PlayerStatsController nearestPlayerStatsController = nearestPlayer.GetComponent<PlayerStatsController>();
            Vector2 targetDirection = (nearestPlayerStatsController.GetPlayerPosition() - transform.position).normalized;
            bool isCloseEnough = Vector2.Distance(transform.position, nearestPlayerStatsController.GetPlayerPosition()) <= minDistanceToPlayer;
            _movementDirection = isCloseEnough ? Vector2.zero : targetDirection;
            
            RunCloserToPlayer(nearestPlayerStatsController);
        }
        else
        {
            _movementDirection = Vector2.zero;
        }
    }

    // Move the enemy based on the movement direction and move speed
    private void MoveEnemy()
    {
        var newPosition = _rb.position + _movementDirection * 
            enemyStatsController.GetMoveSpeed() * Time.fixedDeltaTime;
        _rb.MovePosition(newPosition); // Use MovePosition for smooth physics-based movement.
    }

    // The enemy will run faster towards the player if the player is far away
    private void RunCloserToPlayer(PlayerStatsController nearestPlayerStatsController)
    {
        // Check if the enemy is closer to the player
        if (Vector2.Distance(transform.position, nearestPlayerStatsController.GetPlayerPosition()) < runToDistance)
        {
            enemyStatsController.SetMoveSpeed(_originalMoveSpeed);
        }
        else
        {
            enemyStatsController.SetMoveSpeed(_originalMoveSpeed * runSpeedMultiplier);
        }
    }
    
}