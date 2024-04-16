using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnCycle
{
    public GameObject enemyType;
    public int count;
    public float duration;
    public float spawnRateMultiplier;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int currentCycleIndex;
    [SerializeField] private float cycleTimer;
    [SerializeField] private List<SpawnCycle> cycles;
    [SerializeField] private List<GameObject> bosses;
    [SerializeField] private FloatVariable gameTime;

    private GameObject _mainCamera; //TODO: I think this should be FixedMultiplayerCamera
    private EnemySpawnPosition _enemySpawnPosition;
    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>();


    
    private void Start()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        
        _enemySpawnPosition = GetComponent<EnemySpawnPosition>();
        
        SpawnBosses();
        
        if (cycles.Count > 0) StartCycle();
    }
    
    public List<GameObject> GetAllEnemiesList()
    {
        return allEnemies;
    }
    
    public void RemoveEnemyFromList(GameObject enemy)
    {
        allEnemies.Remove(enemy);
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
    
    private void Update()
    {
        cycleTimer -= Time.deltaTime;
        if (cycleTimer <= 0 && cycles.Count > 0) NextCycle();
    }

    private void StartCycle()
    {
        SpawnCycle currentCycle = cycles[currentCycleIndex];
        cycleTimer = currentCycle.duration;
        for (int i = 0; i < currentCycle.count; i++)
        {
            StartCoroutine(SpawnEnemy(currentCycle, i));
        }
    }

    IEnumerator SpawnEnemy(SpawnCycle cycle, int index)
    {
        yield return new WaitForSeconds(index * cycle.spawnRateMultiplier);
        Vector3 spawnPos = _enemySpawnPosition.CalculateSpawnPosition(_mainCamera.transform.position);
        GameObject enemy = Instantiate(cycle.enemyType, spawnPos, Quaternion.identity, GameManager.GetEnemyParent().transform);
        allEnemies.Add(enemy);
    }

    private void NextCycle()
    {
        currentCycleIndex = (currentCycleIndex + 1) % cycles.Count;
        StartCycle();
    }
    
}

