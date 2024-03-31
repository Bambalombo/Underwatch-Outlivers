using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAbility : MonoBehaviour
{
    [SerializeField] private float defaultCooldown;
    public GameObject wallPrefab;
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 
    private bool isOnCooldown = false; 
    private PlayerStatsController playerStatsController;

    [SerializeField] private float wallSpawnDistance;// Example adjustment
    void Start()
    {
        var grandParent = transform.parent.parent;
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        abilityStats = GetComponent<AbilityStats>();

        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
    }

    private void OnAbilityUsed()
    {
        StartCoroutine(WallCoroutine());
            

        abilityCastHandler.StartCooldown(defaultCooldown,abilityStats.GetAttackCooldown()); //!!!!
    }

    IEnumerator WallCoroutine()
    {
        Vector2 spawnDirection = playerStatsController.GetLastMoveDirection().normalized;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * wallSpawnDistance;
        
        float angle = Mathf.Atan2(spawnDirection.y, spawnDirection.x) * Mathf.Rad2Deg;
        
        GameObject spawnedWall = Instantiate(wallPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 1), Quaternion.Euler(0, 0, angle));
        
        yield return new WaitForSeconds(abilityStats.GetAttackLifetime());
        
        Destroy(spawnedWall);
    }


}
