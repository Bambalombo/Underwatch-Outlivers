using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FixedMultiplayerCamera : MonoBehaviour
{
    //This script is used to make the camera follow all the players in the game, it takes the position of all the players and moves the camera to the center of all the players
    public GameObject[] players;
    [SerializeField] private float normalSmoothTime = 0.05f; //Vi kan sætte smooth time hvis vi vil, men det ser lidt fucked up i single player tbh 
    [SerializeField] private float currentSmoothTime;
    [SerializeField] private float deathSmoothTime = 0.420f;
    [SerializeField] private float deathSmoothDuration = 0.69f;

    private Vector3 velocity;
    private GameManager _gameManager;
    private Coroutine _deathSmoothenCoroutine;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        players = GameManager.GetPlayerGameObjects();
        currentSmoothTime = normalSmoothTime;
    }

    private void OnEnable()
    {
        GameManager.OnPlayerDeath += SmoothenCameraShortly;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDeath -= SmoothenCameraShortly;
        StopAllCoroutines();
        currentSmoothTime = normalSmoothTime;
    }

    private void SmoothenCameraShortly(int i, float f)
    {
        if (_deathSmoothenCoroutine!= null)
            StopCoroutine(_deathSmoothenCoroutine);
        
        _deathSmoothenCoroutine = StartCoroutine(SmoothenMovement());
    }

    private IEnumerator SmoothenMovement()
    {
        float elapsedTime = 0;

        while (elapsedTime < deathSmoothDuration)
        {
            currentSmoothTime = Mathf.Lerp(deathSmoothTime, normalSmoothTime, elapsedTime / deathSmoothDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentSmoothTime = normalSmoothTime;
    }

    void LateUpdate()
    {
        if (players.Length == 0)
            return;
        Move();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint;
        newPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, players.Length == 1 ? 0f : currentSmoothTime);
    }

    public Vector3 GetCenterPoint()
    {
        if (players.Length == 1)
        {
            return players[0].transform.position;
        }

        var playerIndices = _gameManager.GetAlivePlayers();
        var bounds = new Bounds(players[playerIndices[0]].transform.position, Vector3.zero);
        
        foreach (var i in playerIndices)
        //for (int i = 0; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }
        
        return bounds.center;
    }
}