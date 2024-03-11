using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Ensure there's a Rigid body 2D component attached to the GameObject.
public class EnemyMovement : MonoBehaviour
{
    [Header("Component Connections")]
    //[SerializeField] private Vector3Variable playerPosition;
    private Rigidbody2D _rb;
    private Vector2 _movementDirection;
    [SerializeField] private float minDistanceToPlayer = 0.2f;
    [SerializeField] private EnemyStatsController enemyStatsController;
    private PlayerStatsController _playerStatsController;


    private void Awake()
    {
        //TODO: Does not work with more players
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
        
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateMovementDirection();
        MoveEnemy();
    }

    private void UpdateMovementDirection()
    {
        // Calculate the direction vector from the enemy to the player.
        Vector2 targetDirection = (_playerStatsController.GetPlayerPosition() - transform.position).normalized;
        // Determine if the enemy is close enough to stop moving.
        bool isCloseEnough = Vector2.Distance(transform.position, _playerStatsController.GetPlayerPosition()) <= minDistanceToPlayer;
        _movementDirection = isCloseEnough ? Vector2.zero : targetDirection; // Stop moving if close enough, otherwise move towards the player.
    }

    private void MoveEnemy()
    {
        // Calculate the new position based on the movement direction and move speed.
        var newPosition = _rb.position + _movementDirection * 
            enemyStatsController.GetMoveSpeed() * Time.fixedDeltaTime;
        _rb.MovePosition(newPosition); // Use MovePosition for smooth physics-based movement.
    }
}