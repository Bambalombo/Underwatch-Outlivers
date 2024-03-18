using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenziedMutation : MonoBehaviour
{
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 
    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;

    private bool isOnCooldown = false; 

    void Start()
    {
        //Get components from grandparent and current gameobject
        var grandParent = transform.parent.parent;
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        playerHealthController = grandParent.GetComponent<PlayerHealthController>();
        abilityStats = GetComponent<AbilityStats>();
        //Subscribe to event
        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
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
        //playerStatsController.attackSpeed *= abilityStats.attackSpeedMultiplier;
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() * 2f);

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
    }

    private IEnumerator AbilityCooldown()
    {
        isOnCooldown = true; // Set the cooldown flag

        yield return new WaitForSeconds(20f); // Wait for 20 seconds

        isOnCooldown = false; // Reset the cooldown flag
        Debug.Log("Frenzied Mutation is ready to use again.");
    }
}
