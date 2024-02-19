using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject commonEnemy, uncommonEnemy, rareEnemy, epicEnemy, legendaryEnemy;
    
    void Start()
    {
        
    }
    
    private void SpawnRandomEnemy()
    {
        float roll = Random.Range(1, 1000);
        Debug.Log($"Rolled: {roll}.");
        
        switch (roll)
        {
            case < 5:   Instantiate(legendaryEnemy); break;
            case < 50:  Instantiate(epicEnemy); break;
            case < 100: Instantiate(rareEnemy); break;
            case < 250: Instantiate(uncommonEnemy); break;
            default:    Instantiate(commonEnemy); break;
        }
    }

    private IEnumerator SpawnTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
