using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Ensure there's a Rigid body 2D component attached to the GameObject.
public class EnemyController : MonoBehaviour
{
    [Header("Component Connections")]
    [SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float moveSpeed = 5f; 

    private Rigidbody2D _rb;
    private Vector2 _movementDirection;
    private readonly float _minDistanceToPlayer = 0.2f; 

    private void Awake()
    {
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
        Vector2 targetDirection = (playerPosition.value - transform.position).normalized;
        // Determine if the enemy is close enough to stop moving.
        bool isCloseEnough = Vector2.Distance(transform.position, playerPosition.value) <= _minDistanceToPlayer;
        _movementDirection = isCloseEnough ? Vector2.zero : targetDirection; // Stop moving if close enough, otherwise move towards the player.
    }

    private void MoveEnemy()
    {
        // Calculate the new position based on the movement direction and move speed.
        Vector2 newPosition = _rb.position + _movementDirection * moveSpeed * Time.fixedDeltaTime;
        _rb.MovePosition(newPosition); // Use MovePosition for smooth physics-based movement.
    }
}