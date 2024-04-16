using UnityEngine;

public class EnemyTeleportToPlayer : MonoBehaviour
{
    private EnemySpawnPosition _enemySpawnPosition;
    //[SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float maxDistanceFromPlayer;
    private FixedMultiplayerCamera _fixedMultiplayerCamera;


    private void Awake()
    {
        //TODO: Does not work with more players - Should probably be set to the camera position instead
        _fixedMultiplayerCamera = FindObjectOfType<FixedMultiplayerCamera>();
        
        var spawnerEnemyTransform = GameManager.GetSpawnerEnemyControllerParent();
        _enemySpawnPosition = spawnerEnemyTransform.GetComponent<EnemySpawnPosition>();
    }
    
    private void FixedUpdate()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _fixedMultiplayerCamera.GetCenterPoint());

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

        Vector3 spawnPos = _enemySpawnPosition.CalculateSpawnPosition(_fixedMultiplayerCamera.GetCenterPoint());
        
        transform.position = spawnPos;
    }
}