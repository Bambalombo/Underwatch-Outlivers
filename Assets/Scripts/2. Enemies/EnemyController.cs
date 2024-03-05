using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Ensure there's a Rigidbody2D component attached to the GameObject.
public class EnemyController : MonoBehaviour
{
    [Header("Component Connections")]
    [SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float moveSpeed = 5f; 

    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private float minDistanceToPlayer = 0.2f; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        UpdateMovementDirection();
        MoveEnemy();
    }

    void UpdateMovementDirection()
    {
        // Calculate the direction vector from the enemy to the player.
        Vector2 targetDirection = (playerPosition.value - transform.position).normalized;
        // Determine if the enemy is close enough to stop moving.
        bool isCloseEnough = Vector2.Distance(transform.position, playerPosition.value) <= minDistanceToPlayer;
        movementDirection = isCloseEnough ? Vector2.zero : targetDirection; // Stop moving if close enough, otherwise move towards the player.
    }

    void MoveEnemy()
    {
        // Calculate the new position based on the movement direction and move speed.
        Vector2 newPosition = rb.position + movementDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition); // Use MovePosition for smooth physics-based movement.
    }
}