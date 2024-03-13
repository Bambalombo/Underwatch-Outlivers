using UnityEngine;

public class FixedMultiplayerCamera : MonoBehaviour
{
    public GameObject[] players; // Array to hold all the players
    public float smoothTime = 0.5f; // Time for the camera to refocus

    private Vector3 velocity; // Speed of the camera movement
    
    void Start()
    {
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

        // Note: You may want to adjust the Y and/or Z position depending on your game's camera angle and desired view
        newPosition.z = transform.position.z; // Keep the camera's original Z position
        //newPosition.y = transform.position.y; // Keep the camera's original Y position

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if (players.Length == 1)
        {
            return players[0].transform.position;
        }

        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        for (int i = 0; i < players.Length; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }
        return bounds.center;
    }
}