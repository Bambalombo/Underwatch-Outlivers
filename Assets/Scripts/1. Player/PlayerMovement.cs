using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private PlayerStatsController _playerStatsController;
    private Vector2 _moveVector;
    private PlayerInput playerInput;
    


    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log("Move Vector: " + _moveVector);
        _moveVector = context.ReadValue<Vector2>();
    }

    private void Start()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
        if (_playerStatsController == null)
        {
            Debug.LogError("PlayerStatsController component not found on " + gameObject.name);
        }

        playerInput = GetComponent<PlayerInput>(); // Get the PlayerInput component
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on " + gameObject.name);
        }
        else
        {
            
            if (gameObject.name == "Player_1")
            {
                playerInput.SwitchCurrentControlScheme("P1");
            }
            else if (gameObject.name == "Player_2")
            {
                playerInput.SwitchCurrentControlScheme("P2");
            }
            else if (gameObject.name == "Player_3")
            {
                playerInput.SwitchCurrentControlScheme("P3");
            }
            else if (gameObject.name == "Player_4")
            {
                playerInput.SwitchCurrentControlScheme("P4");
            }
            playerInput.SwitchCurrentActionMap(gameObject.name); // Switch the action map to match the GameObject's name
            
            Debug.Log("Current control scheme:" + playerInput.currentControlScheme);
            Debug.Log("Current action map:" + playerInput.currentActionMap);
        }
        
        
        if (_playerStatsController != null)
        {
            _playerStatsController.SetPlayerPosition(transform.position);
        }
        else
        {
            Debug.LogError("PlayerStatsController is null in Start");
        }
    }

    private void Update()
    {
        if (_playerStatsController == null)
        {
            return; // Exit early if there's no PlayerStatsController
        }

        Vector2 move = _moveVector * (_playerStatsController.GetMoveSpeed() * Time.deltaTime);

        transform.Translate(move);
        _playerStatsController.SetPlayerPosition(transform.position);
    }
}