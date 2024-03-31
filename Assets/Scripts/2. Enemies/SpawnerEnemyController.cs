using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemyController : MonoBehaviour
{
    [SerializeField] private GameObject commonEnemy, uncommonEnemy, rareEnemy, epicEnemy, legendaryEnemy;
    [SerializeField] private float spawnInterval = 5f;
    //[SerializeField] private Vector3Variable playerPosition;
    [SerializeField] private float spawnRadius;
    [SerializeField] private float safeZoneRadius = 5f;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private float difficultyFactor = 1f;

    private GameObject _mainCamera;

    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>();
    
    private void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {
        AdjustDifficulty();
    }

    private void AdjustDifficulty()
    {
        difficultyFactor = 1f + gameTime.value / 300f; // Increase difficulty over time
    }
    
    public List<GameObject> GetAllEnemiesList()
    {
        return allEnemies;
    }
    
    public void RemoveEnemyFromList(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }

    private void SpawnRandomEnemy()
    {
        float roll = Random.Range(1, 1000) * difficultyFactor;
        //Vector3 playerPos = playerPosition.value;

        GameObject enemyToSpawn;
        int spawnCount = CalculateSpawnCount(roll, out enemyToSpawn);

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = CalculateSpawnPosition(_mainCamera.transform.position);
            
            //TODO: This could be improved by recycling enemies instead of instantiating new ones
            GameObject enemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity, transform);
            allEnemies.Add(enemy);
        }
    }
    
    private int CalculateSpawnCount(float roll, out GameObject enemyToSpawn)
    {
        int spawnCount;
    
        // Adjust these thresholds as needed for balancing
        const int legendaryThreshold = 950;
        const int epicThreshold = 850;
        const int rareThreshold = 700;
        const int uncommonThreshold = 500;

        // Adjusting spawn counts and types based on the roll
        if (roll >= legendaryThreshold)
        {
            enemyToSpawn = legendaryEnemy;
            spawnCount = Mathf.CeilToInt(2 * difficultyFactor); // Spawn more as difficulty increases
        }
        else if (roll >= epicThreshold)
        {
            enemyToSpawn = epicEnemy;
            spawnCount = Mathf.CeilToInt(4 * difficultyFactor);
        }
        else if (roll >= rareThreshold)
        {
            enemyToSpawn = rareEnemy;
            spawnCount = Mathf.CeilToInt(6 * difficultyFactor);
        }
        else if (roll >= uncommonThreshold)
        {
            enemyToSpawn = uncommonEnemy;
            spawnCount = Mathf.CeilToInt(10 * difficultyFactor);
        }
        else
        {
            enemyToSpawn = commonEnemy;
            spawnCount = Mathf.CeilToInt(15 * difficultyFactor);
        }

        // Ensure there is at least one enemy spawned
        spawnCount = Mathf.Max(spawnCount, 1);

        return spawnCount;
    }


    /*private Vector3 CalculateSpawnPosition(Vector3 playerPos)
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        randomDirection.y = 0;
        float distance = Random.Range(safeZoneRadius, spawnRadius);
        return playerPos + randomDirection * distance;
    }*/

    public Vector2 CalculateSpawnPosition(Vector2 playerPos)
    {
        Vector2 spawnPos;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            // Generate a random angle
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            // Random distance from player within spawn radius, but outside the safe zone
            float distance = Random.Range(safeZoneRadius, spawnRadius);

            // Calculate spawn position using angle and distance
            spawnPos = new Vector2(
                playerPos.x + Mathf.Cos(angle) * distance,
                playerPos.y + Mathf.Sin(angle) * distance
            );

            attempts++;
        } while (!IsValidSpawnPosition(spawnPos) && attempts < maxAttempts);

        // If a valid position isn't found after maxAttempts, fallback to a default
        if (attempts >= maxAttempts)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            spawnPos = new Vector2(
                playerPos.x + Mathf.Cos(angle) * safeZoneRadius,
                playerPos.y + Mathf.Sin(angle) * safeZoneRadius
            );
        }

        return spawnPos;
    }

    private bool IsValidSpawnPosition(Vector2 position)
    {
        // Check if the position is within the spawn radius and outside the safe zone radius
        /*bool isWithinSpawnRadius = Physics2D.OverlapCircle(position, spawnRadius) != null;
        bool isOutsideSafeZoneRadius = Physics2D.OverlapCircle(position, safeZoneRadius) == null;

        return isWithinSpawnRadius && isOutsideSafeZoneRadius;*/

        return true; // Temporarily returning true
    }


    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnInterval / difficultyFactor);
        }
    }
}
