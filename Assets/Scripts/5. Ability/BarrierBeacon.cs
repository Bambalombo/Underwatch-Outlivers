using UnityEngine;
using System.Collections;

public class BarrierBeacon : MonoBehaviour
{
    [SerializeField] private GameObject buffFieldPrefab; // Assign this in the Unity Inspector
    [SerializeField] private float defaultCooldown = 20f; // Cooldown of the ability
    [SerializeField] private float barrierSize; // Size of the barrier
    
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 
    private PlayerStatsController playerStatsController;
    private PlayerHealthController _playerHealthController;

    private bool isOnCooldown = false;
    
    void Start()
    {
        var grandParent = transform.parent.parent;
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        _playerHealthController = grandParent.GetComponent<PlayerHealthController>();
        abilityStats = GetComponent<AbilityStats>();

        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
    }

    // Method to be called by Unity's input system or other scripts to use the ability
    public void OnAbilityUsed()
    {
        StartCoroutine(SpawnBuffField());
        abilityCastHandler.StartCooldown(defaultCooldown, abilityStats.GetAttackCooldown());
    }

    private IEnumerator SpawnBuffField()
    {
        // Instantiate the buff field at the player's current position
        GameObject buffField = Instantiate(buffFieldPrefab, transform.position, Quaternion.identity);
        buffField.transform.localScale = new Vector3(barrierSize,barrierSize,1);
        Destroy(buffField, abilityStats.GetAttackLifetime()); // Destroy the field after its duration ends
        yield return null;
    }
    
}