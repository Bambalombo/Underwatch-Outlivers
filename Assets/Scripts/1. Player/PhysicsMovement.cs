using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsMovement : MonoBehaviour
{
    private PlayerStatsController _playerStatsController;
    private Rigidbody2D _rb;
    private Vector2 _movementDirection;
    private Vector2 _lastMovementAngle;
    
    private void Awake()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
    }
    
    //This code is only for when using controllers, since they can move in all 360 degrees and the melee weapon works best when snapping to 45 degree angles i think
    private Vector2 SnapDirectionToNearestAngle(Vector2 direction, float snapAngle)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Convert direction to angle
        var snappedAngle = Mathf.Round(angle / snapAngle) * snapAngle; // Snap angle
        var snappedRadians = snappedAngle * Mathf.Deg2Rad; // Convert back to radians for trigonometry
        return new Vector2(Mathf.Cos(snappedRadians), Mathf.Sin(snappedRadians)); // Convert back to vector
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerStatsController = GetComponent<PlayerStatsController>();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementDirection = context.ReadValue<Vector2>();
        // Normalize the movement direction only if it's not zero
        if (_movementDirection != Vector2.zero)
        {
            var snappedDirection = SnapDirectionToNearestAngle(_movementDirection, 45); // Snap to nearest 45-degree angle
            _lastMovementAngle = snappedDirection.normalized; // Update last move direction
            _playerStatsController.SetLastMoveDirection(_lastMovementAngle);
        }
        //Debug.Log($"Last move direction: {_playerStatsController.GetLastMoveDirection()}");
    }

    void FixedUpdate()
    {
        _rb.velocity = _movementDirection * _playerStatsController.GetMoveSpeed();
        _playerStatsController.SetPlayerPosition(gameObject.transform.position);
    }
    
    
}