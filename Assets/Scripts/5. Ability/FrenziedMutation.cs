using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenziedMutation : MonoBehaviour
{
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 
    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private ClassAssets classAssets;
    private WeaponStats weaponStats;
    private GameObject currentWeapon;
    
    public GameObject frenzyEffectPrefab; // The particle effect for the ability

    private bool isOnCooldown = false; 

    void Start()
    {
        //Get components from grandparent and current gameobject
        var grandParent = transform.parent.parent;
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        playerHealthController = grandParent.GetComponent<PlayerHealthController>();
        abilityStats = GetComponent<AbilityStats>();
        classAssets = grandParent.GetComponent<ClassAssets>();
        //Subscribe to event
        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
        
        currentWeapon = classAssets.GetActiveWeapons();
        weaponStats = currentWeapon.GetComponent<WeaponStats>();
    }

    private void OnAbilityUsed()
    {
        if (isOnCooldown)
        {
            Debug.Log("Ability is on cooldown.");
            return; // Exit if the ability is on cooldown
        }

        Debug.Log("Ability used! Frenzied Mutation activated.");

        StartCoroutine(ActivateFrenziedMutation());
        StartCoroutine(AbilityCooldown());
    }

    private IEnumerator ActivateFrenziedMutation()
    {
        // Start the particle effect
        GameObject frenzyEffect = Instantiate(frenzyEffectPrefab, playerStatsController.transform.position, Quaternion.identity, playerStatsController.transform);
        
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() * 2f); // Movespeed
        weaponStats.SetDamage(weaponStats.GetDamage() * 2f); // Damage
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown()/2f); // Attack speed/weapon cooldown

        // Duration of the ability effect
        float duration = 10f; // 10 seconds

        while (duration > 0)
        {
            // Slowly drain health over the 10-second period
            playerHealthController.PlayerTakeDamage(5f * Time.deltaTime);
            duration -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Revert stats back to normal after the effect ends
        //playerStatsController.weaponDamage /= abilityStats.damageMultiplier;
        //playerStatsController.attackSpeed /= abilityStats.attackSpeedMultiplier;
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() / 2f);
        weaponStats.SetDamage(weaponStats.GetDamage() / 2f);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() * 2f);
        
        // Stop the particle effect
        Destroy(frenzyEffect);
    }

    private IEnumerator AbilityCooldown()
    {
        isOnCooldown = true; // Set the cooldown flag

        yield return new WaitForSeconds(20f); // Wait for 20 seconds

        isOnCooldown = false; // Reset the cooldown flag
        Debug.Log("Frenzied Mutation is ready to use again.");
    }
}
