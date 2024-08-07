using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NearestEnemyFinder : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    
    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
    }

    public GameObject GetNearestEnemy(Vector2 position, List<GameObject> enemies = null)
    {
        // Get the list with exactly 1 nearest enemy
        List<GameObject> nearestEnemies = GetClosestEnemies(position, 1, enemies);

        // Return the first and only enemy if available, otherwise null
        return nearestEnemies.FirstOrDefault();
    }

    public List<GameObject> GetClosestEnemies(Vector2 position, int numberOfEnemies, List<GameObject> enemies = null, float maxRange = 1000)
    {
        if (enemies == null)
            enemies = _enemySpawner.GetAllEnemiesList();

        // Create a list to store enemies and their distances
        List<(GameObject enemy, float distance)> closestEnemies = new List<(GameObject, float)>();

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue; // Skip if the enemy has been destroyed

            float distance = Vector2.Distance(position, enemy.transform.position);
        
            if (distance <= maxRange) // Only add the enemy if within the specified range
            {
                closestEnemies.Add((enemy, distance));
            }
        }

        // Sort the list by distance and take the top 'numberOfEnemies' entries
        closestEnemies = closestEnemies.OrderBy(x => x.distance).ToList();

        // Return only the GameObject part of the tuple, up to the specified number of closest enemies
        return closestEnemies.Take(numberOfEnemies).Select(x => x.enemy).ToList();

    }

    public List<GameObject> GetChainOfEnemies(Vector3 lastPos, float targetCount)
    {
        var enemyHitList = new List<GameObject>();
        var enemyLookList = new List<GameObject>();
        enemyLookList.AddRange(_enemySpawner.GetAllEnemiesList());
        
        for (int i = 0; i < (int)targetCount; i++)
        {
            // we get the nearest enemy and save it in the "currentEnemy variable"
            var currentEnemy = GetNearestEnemy(lastPos, enemyLookList);
            if (currentEnemy == null) break;
            
            // we then add the current enemy to the list of enemies to be hit and removes it from the look list
            enemyHitList.Add(currentEnemy);
            enemyLookList.Remove(currentEnemy);

            // lastly, we update lastPos to the current enemy position, and also add it to pos list
            lastPos = currentEnemy.transform.position;
        }

        return enemyHitList;
    }
    
    public List<GameObject> GetChainOfEnemiesInProximity(Vector3 startPos, float targetCount, float proximityRange)
    {
        var enemyHitList = new List<GameObject>();
        var enemyLookList = new List<GameObject>();
        enemyLookList.AddRange(_enemySpawner.GetAllEnemiesList());
        var lastPos = startPos;

        if (enemyLookList.Count < targetCount)
            targetCount = enemyLookList.Count;
        
        for (int i = 0; i < (int)targetCount; i++)
        {
            // we get the nearest enemy and save it in the "currentEnemy variable"
            var currentEnemy = GetNearestEnemy(lastPos, enemyLookList);
            //Debug.Log($"{currentEnemy}, {lastPos}");
            
            float distance = Vector2.Distance(lastPos, currentEnemy.transform.position);
            if (distance > proximityRange || currentEnemy == null) break;
            
            // we then add the current enemy to the list of enemies to be hit and removes it from the look list
            enemyHitList.Add(currentEnemy);
            enemyLookList.Remove(currentEnemy);

            // lastly, we update lastPos to the current enemy position, and also add it to pos list
            lastPos = currentEnemy.transform.position;
        }

        return enemyHitList;
    }
    
    public Dictionary<List<GameObject>,List<Vector3>> GetChainOfEnemiesAndPositions(Vector3 lastPos, float targetCount)
    {
        var enemyLookList = new List<GameObject>();
        enemyLookList.AddRange(_enemySpawner.GetAllEnemiesList());
        var enemyHitList = new List<GameObject>();
        var enemyPosList = new List<Vector3>();
        
        for (int i = 0; i < (int)targetCount; i++)
        {
            // we get the nearest enemy and save it in the "currentEnemy variable"
            var currentEnemy = GetNearestEnemy(lastPos, enemyLookList);
            if (currentEnemy == null)
                break;
            
            // we then add the current enemy to the list of enemies to be hit and removes it from the look list
            //Debug.Log($"{enemyHitList.Count} - {currentEnemy}");
            enemyHitList.Add(currentEnemy);
            enemyLookList.Remove(currentEnemy);

            // lastly, we update lastPos to the current enemy position, and also add it to pos list
            lastPos = currentEnemy.transform.position;
            enemyPosList.Add(lastPos);
        }

        Dictionary<List<GameObject>, List<Vector3>> enemiesAndPositions = new Dictionary<List<GameObject>, List<Vector3>>();
        enemiesAndPositions.Add(enemyHitList, enemyPosList);
        return enemiesAndPositions;
    }
}