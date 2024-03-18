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
    
    [SerializeField] private PlayerStatsController playerStatsController;

    private void Awake()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
        movementSpeed = _playerStatsController.GetMoveSpeed();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //Get the playerstats controller attached to this gameobject
        playerStatsController = GetComponent<PlayerStatsController>();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(movementDirection.x, movementDirection.y);
        playerStatsController.SetLastMoveDirection(movementDirection);
    }



    void FixedUpdate()
    {
        rb.velocity = movementDirection * movementSpeed;

        _playerStatsController.SetPlayerPosition(gameObject.transform.position);
    }
}
