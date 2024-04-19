using UnityEngine;

public class WeaponDrone : MonoBehaviour
{
    private PlayerStatsController _playerStatsController;
    [SerializeField] private GameObject dronePrefab;
    private WeaponStats _weaponStats;
    private Vector3 _lastPlayerPosition;
    private int arrayMax;
    private int soundToPlay;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private float totalDistanceMoved;
    [SerializeField] private AudioClip[] arraySounds;
    
    //Talent variables
    public bool shockSphereEnabled;
    
    private void Awake()
    {
        var grandParent = transform.parent.parent;
        _playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        
        _lastPlayerPosition = _playerStatsController.GetPlayerPosition();

        _weaponStats = GetComponent<WeaponStats>();
        
        arrayMax = arraySounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
        
    }

    private void FixedUpdate()
    {
        Vector3 currentPlayerPosition = _playerStatsController.GetPlayerPosition();

        float distanceMoved = Vector3.Distance(_lastPlayerPosition, currentPlayerPosition);

        totalDistanceMoved += distanceMoved;

        if (totalDistanceMoved >= _weaponStats.GetAttackCooldown()*4) 
        {
            SpawnDrone(); // Spawn the drone
            soundToPlay = Random.Range(0, arrayMax);
            audioSource.clip = arraySounds[soundToPlay];
            audioSource.Play();
            
            totalDistanceMoved = 0f; // Reset the total distance moved
        }

        _lastPlayerPosition = currentPlayerPosition;
    }

    private void SpawnDrone()
    {
        GameObject drone = Instantiate(dronePrefab, _playerStatsController.GetPlayerPosition(), Quaternion.identity, GameManager.GetBulletParent());
        if (shockSphereEnabled)
        {
            gameObject.GetComponent<AoeDamagePool>().duration = _weaponStats.GetAttackLifetime();
            gameObject.GetComponent<AoeDamagePool>().AttemptInitialize(_weaponStats.GetDamage() / 10,
                _playerStatsController.GetPlayerPosition());
        }
        
        Drones dronesScript = drone.GetComponent<Drones>();

        dronesScript.GetWeaponStats(_weaponStats); // Pass the WeaponStats script to the drone
        
        Destroy(drone, _weaponStats.GetAttackLifetime()); // Destroy the drone after x seconds
    }
}