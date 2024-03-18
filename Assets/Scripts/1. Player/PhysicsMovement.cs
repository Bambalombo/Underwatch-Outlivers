using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsMovement : MonoBehaviour
{
    private float movementSpeed;
    private PlayerStatsController _playerStatsController;
    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private Vector2 lastMovementAngle;
    
    [SerializeField] private PlayerStatsController playerStatsController;

    private void Awake()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
        movementSpeed = _playerStatsController.GetMoveSpeed();
    }
    
    private Vector2 SnapDirectionToNearestAngle(Vector2 direction, float snapAngle)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert direction to angle
        var snappedAngle = Mathf.Round(angle / snapAngle) * snapAngle; // Snap angle
        var snappedRadians = snappedAngle * Mathf.Deg2Rad; // Convert back to radians for trigonometry
        return new Vector2(Mathf.Cos(snappedRadians), Mathf.Sin(snappedRadians)); // Convert back to vector
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStatsController = GetComponent<PlayerStatsController>();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
        // Normalize the movement direction only if it's not zero
        if (movementDirection != Vector2.zero)
        {
            var snappedDirection = SnapDirectionToNearestAngle(movementDirection, 45); // Snap to nearest 45-degree angle
            lastMovementAngle = snappedDirection.normalized; // Update last move direction
            playerStatsController.SetLastMoveDirection(lastMovementAngle);
        }
        Debug.Log($"Last move direction: {playerStatsController.GetLastMoveDirection()}");
    }

    void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;
        _playerStatsController.SetPlayerPosition(gameObject.transform.position);
    }
    
    
}