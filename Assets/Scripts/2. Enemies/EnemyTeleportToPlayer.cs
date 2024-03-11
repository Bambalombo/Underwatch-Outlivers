using UnityEngine;

public class EnemyTeleportToPlayer : MonoBehaviour
{
    private SpawnerEnemyController _spawnerEnemyController;
    //[SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float maxDistanceFromPlayer;
    private PlayerStatsController _playerStatsController;


    private void Awake()
    {
        //TODO: Does not work with more players
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
    }

    private void Start()
    {
        //TODO: Find a way to get the reference to the EnemySpawner without using FindObjectOfType
        _spawnerEnemyController = FindObjectOfType<SpawnerEnemyController>();
    }
    private void FixedUpdate()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _playerStatsController.GetPlayerPosition());

        // Check if the enemy is too far from the player
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            TeleportEnemy();
        }
    }

    private void TeleportEnemy()
    {
        // Generate a random position within the spawn radius but outside the safe zone
        //Vector2 randomDirection = Random.insideUnitCircle.normalized;
        //Vector3 spawnPos = playerPosition.value + new Vector3(randomDirection.x, 0, randomDirection.y) * Random.Range(enemySpawner.SafeZoneRadius + 1, enemySpawner.SpawnRadius);

        Vector3 spawnPos = _spawnerEnemyController.CalculateSpawnPosition(_playerStatsController.GetPlayerPosition());
        
        transform.position = spawnPos;
    }
}