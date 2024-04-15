using UnityEngine;

public class WeaponDrone : MonoBehaviour
{
    private PlayerStatsController _playerStatsController;
    [SerializeField] private GameObject dronePrefab;
    private WeaponStats _weaponStats;
    private Vector3 _lastPlayerPosition;
    [SerializeField] private float totalDistanceMoved;
    
    private void Awake()
    {
        var grandParent = transform.parent.parent;
        _playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        
        _lastPlayerPosition = _playerStatsController.GetPlayerPosition();

        _weaponStats = GetComponent<WeaponStats>();
    }

    private void FixedUpdate()
    {
        Vector3 currentPlayerPosition = _playerStatsController.GetPlayerPosition();

        float distanceMoved = Vector3.Distance(_lastPlayerPosition, currentPlayerPosition);

        totalDistanceMoved += distanceMoved;

        if (totalDistanceMoved >= _weaponStats.GetAttackCooldown()) 
        {
            SpawnDrone(); // Spawn the drone
            
            totalDistanceMoved = 0f; // Reset the total distance moved
        }

        _lastPlayerPosition = currentPlayerPosition;
    }

    private void SpawnDrone()
    {
        GameObject drone = Instantiate(dronePrefab, _playerStatsController.GetPlayerPosition(), Quaternion.identity, GameManager.GetBulletParent());

        Drones dronesScript = drone.GetComponent<Drones>();

        dronesScript.GetWeaponStats(_weaponStats); // Pass the WeaponStats script to the drone
        
        Destroy(drone, _weaponStats.GetAttackLifetime()); // Destroy the drone after x seconds
    }
}