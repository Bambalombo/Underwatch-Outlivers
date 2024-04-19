using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnCycle
{
    public GameObject enemyType; // Enemy prefab to spawn
    public int startCount;  // Initial number of enemies to spawn at the start
    public int peakCount;   // Maximum number of enemies to spawn towards the end
    public float startTime; // Game time when the cycle should start
    public float endTime;   // Game time when the cycle should end
    public float spawnDelay;  // Delay between spawns
}

public class EnemySpawner : MonoBehaviour
{
    public List<SpawnCycle> cycles;
    private List<GameObject> allEnemies = new List<GameObject>();
    private EnemySpawnPosition _enemySpawnPosition;
    private float lastSpawnTime = -1;
    private GameObject _mainCamera;
    [SerializeField] private List<GameObject> bosses;
    [SerializeField] private FloatVariable gameTime;

    private void Start()
    {
        _enemySpawnPosition = GetComponent<EnemySpawnPosition>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        SpawnBosses();
    }

    private void Update()
    {
        float currentTime = gameTime.value;
        foreach (var cycle in cycles)
        {
            if (currentTime >= cycle.startTime && currentTime <= cycle.endTime)
            {
                if (lastSpawnTime < 0 || currentTime >= lastSpawnTime + cycle.spawnDelay)
                {
                    SpawnEnemies(cycle, currentTime);
                    lastSpawnTime = currentTime;
                }
            }
        }
    }

    private void SpawnEnemies(SpawnCycle cycle, float currentTime)
    {
        float progress = (currentTime - cycle.startTime) / (cycle.endTime - cycle.startTime);
        int currentCount = Mathf.FloorToInt(Mathf.Lerp(cycle.startCount, cycle.peakCount, progress));
        Debug.Log("Current count: " + currentCount);
        
        for (int i = 0; i < currentCount; i++)
        {
            Vector2 spawnPosition = _enemySpawnPosition.CalculateSpawnPosition(_mainCamera.transform.position);

            GameObject enemy = Instantiate(cycle.enemyType, spawnPosition, Quaternion.identity, GameManager.GetEnemyParent());
            allEnemies.Add(enemy);
            //Debug.Log($"Spawning enemy at {spawnPosition} with {cycle.enemyType.name}");
        }
    }
    
    private void SpawnBosses()
    {
        foreach (var boss in bosses)
        {
            Vector3 spawnPosition = _enemySpawnPosition.RandomBossSpawnPosition();
            GameObject bossObj = Instantiate(boss, spawnPosition, Quaternion.identity, 
                GameManager.GetBossParent().transform);
            allEnemies.Add(bossObj);
        }
    }

    public List<GameObject> GetAllEnemiesList()
    {
        return allEnemies;
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        allEnemies.Remove(enemy);
    }
}
