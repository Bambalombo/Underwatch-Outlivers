using System.Collections.Generic;
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
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        if (enemies == null)
            enemies = _enemySpawner.GetAllEnemiesList();
        
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue; // Skip if the enemy has been destroyed

            float distance = Vector2.Distance(position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
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
        
        for (int i = 0; i < (int)targetCount; i++)
        {
            // we get the nearest enemy and save it in the "currentEnemy variable"
            var currentEnemy = GetNearestEnemy(lastPos, enemyLookList);
            
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