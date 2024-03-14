using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _moveVector;
    //[SerializeField] private FloatVariable playerMoveSpeed;
    //[SerializeField] private Vector3Variable playerPosition;
    private PlayerStatsController _playerStatsController;

    private void Awake()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
    }

    private void Start()
    {
        //_playerStatsController.SetPlayerPosition(transform.position);
    }

    private void Update()
    {
        var playerTransform = transform;

        _moveVector.x = Input.GetAxisRaw("Horizontal");
        _moveVector.y = Input.GetAxisRaw("Vertical");

        playerTransform.Translate(_moveVector.normalized * (_playerStatsController.GetMoveSpeed() * Time.deltaTime));
        //_playerStatsController.SetPlayerPosition(playerTransform.position);
    }
}
