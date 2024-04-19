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
    [SerializeField] private List<SpawnCycle> cycles;
    [SerializeField] private List<GameObject> bosses;
    [SerializeField] private FloatVariable gameTime;

    private GameObject _mainCamera;
    private EnemySpawnPosition _enemySpawnPosition;
    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>();
    private Dictionary<SpawnCycle, Coroutine> activeSpawns = new Dictionary<SpawnCycle, Coroutine>();

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
            bool isCycleActive = currentTime >= cycle.startTime && currentTime < cycle.endTime;
            bool isSpawnActive = activeSpawns.ContainsKey(cycle);

            if (isCycleActive && !isSpawnActive)
            {
                activeSpawns[cycle] = StartCoroutine(SpawnCycleEnemies(cycle));
            }
            else if (!isCycleActive && isSpawnActive)
            {
                StopCoroutine(activeSpawns[cycle]);
                activeSpawns.Remove(cycle);
            }
        }
    }

    private IEnumerator SpawnCycleEnemies(SpawnCycle cycle)
    {
        float elapsed = 0; // Time elapsed since the start of the spawning cycle
        int spawnedSoFar = 0; // Number of enemies spawned so far

        while (elapsed < cycle.endTime - cycle.startTime)
        {
            yield return new WaitForSeconds(cycle.spawnDelay);
            elapsed += cycle.spawnDelay;
            int targetCount = Mathf.FloorToInt(Mathf.Lerp(cycle.startCount, cycle.peakCount, elapsed / (cycle.endTime - cycle.startTime)));
            int toSpawn = targetCount - spawnedSoFar;

            for (int i = 0; i < toSpawn; i++)
            {
                Vector3 spawnPosition = _enemySpawnPosition.CalculateSpawnPosition(_mainCamera.transform.position);
                GameObject enemy = Instantiate(cycle.enemyType, spawnPosition, Quaternion.identity, GameManager.GetEnemyParent().transform);
                Debug.Log("Spawning enemy at " + spawnPosition + " with " + cycle.enemyType.name);
                allEnemies.Add(enemy);
            }
            spawnedSoFar += toSpawn;
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
