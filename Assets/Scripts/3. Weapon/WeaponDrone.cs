using UnityEngine;

public class WeaponDrone : MonoBehaviour
{
    private PlayerStatsController _playerStatsController;
    [SerializeField] private GameObject dronePrefab;
    private WeaponStats _weaponStats;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip[] arraySounds;

    // Configuration Variables
    [SerializeField] private bool useTimeBasedSpawning = true; // Toggle this in the inspector
    private Vector3 _lastPlayerPosition;
    private float totalDistanceMoved;
    private float spawnTimer;

    // Talent variables
    public bool shockSphereEnabled;

    // Timer variables
    private int arrayMax;
    private int soundToPlay;

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

    private void Update()
    {
        if (useTimeBasedSpawning)
        {
            // Time-based spawning logic
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= _weaponStats.GetAttackCooldown())
            {
                TriggerSpawn();
            }
        }
        else
        {
            // Distance-based spawning logic
            Vector3 currentPlayerPosition = _playerStatsController.GetPlayerPosition();
            float distanceMoved = Vector3.Distance(_lastPlayerPosition, currentPlayerPosition);
            totalDistanceMoved += distanceMoved;
            
            if (totalDistanceMoved >= _weaponStats.GetAttackCooldown() * 4) // Adjust the multiplier as needed
            {
                TriggerSpawn();
            }
            _lastPlayerPosition = currentPlayerPosition;
        }
    }

    private void TriggerSpawn()
    {
        SpawnDrone(); // Spawn the drone
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
        audioSource.Play();

        // Reset timers and distance counters
        spawnTimer = 0f;
        totalDistanceMoved = 0f;
    }

    private void SpawnDrone()
    {
        GameObject drone = Instantiate(dronePrefab, _playerStatsController.GetPlayerPosition(), Quaternion.identity, GameManager.GetBulletParent());
        if (shockSphereEnabled)
        {
            gameObject.GetComponent<AoeDamagePool>().duration = _weaponStats.GetAttackLifetime();
            gameObject.GetComponent<AoeDamagePool>().AttemptInitialize(_weaponStats.GetDamage() / 10, _playerStatsController.GetPlayerPosition());
        }
        
        Drones dronesScript = drone.GetComponent<Drones>();
        dronesScript.GetWeaponStats(_weaponStats); // Pass the WeaponStats script to the drone
        
        Destroy(drone, _weaponStats.GetAttackLifetime()); // Destroy the drone after its lifetime
    }
}
