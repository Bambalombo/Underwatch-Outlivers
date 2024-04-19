using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnCycle
{
    public GameObject enemyType; // Enemy prefab to spawn
    public int startCount;  // Initial number of enemies to spawn
    public int peakCount;   // Maximum number of enemies to spawn as the cycle progresses
    public float startTime; // Game time when the cycle should start
    public float endTime;   // Game time when the cycle should end
    public float spawnRateMultiplier; // Multiplier for the spawn rate
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnCycle> cycles;
    [SerializeField] private List<GameObject> bosses;
    [SerializeField] private FloatVariable gameTime;
    
    private GameObject _mainCamera;
    private EnemySpawnPosition _enemySpawnPosition;
    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>();

    private void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _enemySpawnPosition = GetComponent<EnemySpawnPosition>();
        SpawnBosses();
    }

    private void Update()
    {
        float currentTime = gameTime.value;
        
        foreach (var cycle in cycles)
        {
            if (currentTime >= cycle.startTime && currentTime < cycle.endTime)
            {
                float cycleProgress = (currentTime - cycle.startTime) / (cycle.endTime - cycle.startTime);
                int spawnCount = (int)Mathf.Lerp(cycle.startCount, cycle.peakCount, cycleProgress);
                ManageSpawning(cycle, spawnCount);
            }
        }
    }

    private void ManageSpawning(SpawnCycle cycle, int count)
    {
        StopAllCoroutines(); // Stop existing spawn coroutines if needed
        for (int i = 0; i < count; i++)
        {
            StartCoroutine(SpawnEnemy(cycle, i * cycle.spawnRateMultiplier));
        }
    }

    private IEnumerator SpawnEnemy(SpawnCycle cycle, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 spawnPos = _enemySpawnPosition.CalculateSpawnPosition(_mainCamera.transform.position);
        GameObject enemy = Instantiate(cycle.enemyType, spawnPos, Quaternion.identity, GameManager.GetEnemyParent().transform);
        allEnemies.Add(enemy);
    }

    private void SpawnBosses()
    {
        foreach (var boss in bosses)
        {
            Vector3 spawnPos = _enemySpawnPosition.RandomBossSpawnPosition();
            var bossObj = Instantiate(boss, spawnPos, Quaternion.identity, GameManager.GetBossParent().transform);
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
