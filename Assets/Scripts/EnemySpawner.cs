using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject commonEnemy, uncommonEnemy, rareEnemy, epicEnemy, legendaryEnemy;
    [SerializeField] private float spawnInterval = 5f; // Time between enemy spawns
    [SerializeField] private GameObject player; // The player's position
    [SerializeField] private float spawnRadius; // The radius around the player where enemies can spawn
    [SerializeField]private float gameTime = 0f; // Total game time

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        gameTime += Time.deltaTime; // Update game time
    }

    private void SpawnRandomEnemy()
    {
        float roll = Random.Range(1, 1000);
        Debug.Log($"Rolled: {roll}.");

        // Get the player's position
        Vector3 playerPos = player.transform.position;

        // Calculate a random angle
        float angle = Random.Range(0, 360);

        // Calculate the position on a circle around the player
        Vector3 spawnPos = playerPos + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRadius;

        GameObject enemyToSpawn;
        int spawnCount;

        // Adjust spawn count and enemy types based on game time
        if (gameTime < 300) // First 5 minutes
        {
            switch (roll)
            {
                case < 5:
                    enemyToSpawn = legendaryEnemy;
                    spawnCount = 1;
                    break;
                case < 50:
                    enemyToSpawn = epicEnemy;
                    spawnCount = 1;
                    break;
                case < 100:
                    enemyToSpawn = rareEnemy;
                    spawnCount = 1;
                    break;
                case < 250:
                    enemyToSpawn = uncommonEnemy;
                    spawnCount = 15;
                    break;
                default:
                    enemyToSpawn = commonEnemy;
                    spawnCount = 100;
                    break;
            }
        }
        else if (gameTime < 600) // Next 5 minutes
        {
            switch (roll)
            {
                case < 10:
                    enemyToSpawn = legendaryEnemy;
                    spawnCount = 2;
                    break;
                case < 100:
                    enemyToSpawn = epicEnemy;
                    spawnCount = 2;
                    break;
                case < 200:
                    enemyToSpawn = rareEnemy;
                    spawnCount = 2;
                    break;
                case < 500:
                    enemyToSpawn = uncommonEnemy;
                    spawnCount = 30;
                    break;
                default:
                    enemyToSpawn = commonEnemy;
                    spawnCount = 200;
                    break;
            }
        }
        else // After 10 minutes
        {
            switch (roll)
            {
                case < 20:
                    enemyToSpawn = legendaryEnemy;
                    spawnCount = 3;
                    break;
                case < 200:
                    enemyToSpawn = epicEnemy;
                    spawnCount = 3;
                    break;
                case < 400:
                    enemyToSpawn = rareEnemy;
                    spawnCount = 3;
                    break;
                case < 800:
                    enemyToSpawn = uncommonEnemy;
                    spawnCount = 45;
                    break;
                default:
                    enemyToSpawn = commonEnemy;
                    spawnCount = 300;
                    break;
            }
        }

        // Instantiate the enemies at the calculated position
        for (int i = 0; i < spawnCount; i++)
        {
            // For clumps, add a small random offset to the spawn position
            Vector3 offset = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Vector3 clumpSpawnPos = spawnPos + offset;

            // For circle, distribute the spawn positions evenly around the circle
            float circleAngle = angle + (360f / spawnCount) * i;
            Vector3 circleSpawnPos = playerPos + new Vector3(Mathf.Cos(circleAngle), 0, Mathf.Sin(circleAngle)) * spawnRadius;

            // Choose the spawn position based on the enemy type
            Vector3 finalSpawnPos = (spawnCount == 1) ? clumpSpawnPos : circleSpawnPos;

            // Instantiate the enemy at the final spawn position
            Instantiate(enemyToSpawn, finalSpawnPos, Quaternion.identity, transform);
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
