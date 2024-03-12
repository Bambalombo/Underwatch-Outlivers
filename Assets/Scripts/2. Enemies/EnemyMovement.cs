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


    private void Awake()
    {
        var players = GameManager.GetPlayerGameObjects();
        playerStatsControllers = new PlayerStatsController[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            playerStatsControllers[i] = players[i].GetComponent<PlayerStatsController>();
        }

        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateMovementDirection();
        MoveEnemy();
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
}