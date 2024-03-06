using UnityEngine;

public class EnemyTeleportToPlayer : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    [SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float maxDistanceFromPlayer = 20f; 

    void Start()
    {
        //TODO: Find a way to get the reference to the EnemySpawner without using FindObjectOfType
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.value);

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

        Vector3 spawnPos = _enemySpawner.CalculateSpawnPosition(playerPosition.value);
        
        transform.position = spawnPos;
    }
}