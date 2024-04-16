using System.Collections.Generic;
using UnityEngine;

public class FixedMultiplayerCamera : MonoBehaviour
{
    //This script is used to make the camera follow all the players in the game, it takes the position of all the players and moves the camera to the center of all the players
    public GameObject[] players;
    public float smoothTime = 0; //Vi kan sætte smooth time hvis vi vil, men det ser lidt fucked up i single player tbh 

    private Vector3 velocity;
    private GameManager _gameManager;
    
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        players = GameManager.GetPlayerGameObjects();
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

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, players.Length == 1 ? 0f : smoothTime);
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