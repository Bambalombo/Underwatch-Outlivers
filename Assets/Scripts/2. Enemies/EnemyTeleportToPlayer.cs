using UnityEngine;

public class EnemyTeleportToPlayer : MonoBehaviour
{
    private SpawnerEnemyController _spawnerEnemyController;
    //[SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float maxDistanceFromPlayer;
    private PlayerStatsController _playerStatsController;


    private void Awake()
    {
        //TODO: Does not work with more players - Should probably be set to the camera position instead
        _playerStatsController = FindObjectOfType<PlayerStatsController>();
        
        var spawnerEnemyTransform = GameManager.GetSpawnerEnemyControllerParent();
        _spawnerEnemyController = spawnerEnemyTransform.GetComponent<SpawnerEnemyController>();
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