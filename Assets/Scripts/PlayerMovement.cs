using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    private Vector3 _moveVector;
    [SerializeField] private FloatVariable playerMoveSpeed;
    [SerializeField] private Vector3Variable playerPosition;

    private void Start()
    {
        playerPosition.value = transform.position;
    }

    private void Update()
    {
        Transform playerTransform = transform;

        _moveVector.x = Input.GetAxisRaw("Horizontal");
        _moveVector.y = Input.GetAxisRaw("Vertical");

        playerTransform.Translate(_moveVector.normalized * (playerMoveSpeed.value * Time.deltaTime));
        playerPosition.value = playerTransform.position;
    }
}
