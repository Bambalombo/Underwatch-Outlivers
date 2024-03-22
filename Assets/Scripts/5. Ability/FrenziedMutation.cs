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
    
    [SerializeField] private float defaultCooldown;
    [SerializeField] private float defaultAbilityDuration;
    
    public GameObject frenzyEffectPrefab;

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
        StartCoroutine(ActivateFrenziedMutation());
        
        // DEN HER MÅDE AT STARTE COOLDOWN PÅ SKAL HELST BRUGES I DE ANDRE ABILITY SCRIPTS OGSÅ 
        abilityCastHandler.StartCooldown(defaultCooldown,abilityStats.GetAttackCooldown()); //!!!!
    }

    private IEnumerator ActivateFrenziedMutation()
    {
        // Start the particle effect
        GameObject frenzyEffect = Instantiate(frenzyEffectPrefab, playerStatsController.transform.position, Quaternion.identity, playerStatsController.transform);
        
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() * 2f); // Movespeed
        weaponStats.SetDamage(weaponStats.GetDamage() * 2f); // Damage
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown()/2f); // Attack speed/weapon cooldown

        // Duration of the ability effect
        float duration = defaultAbilityDuration; // 10 seconds

        while (duration > 0)
        {
            // Slowly drain health over the 10-second period
            playerHealthController.PlayerTakeDamage(5f * Time.deltaTime);
            duration -= Time.deltaTime;
            yield return null;
        }
        //playerStatsController.weaponDamage /= abilityStats.damageMultiplier;
        //playerStatsController.attackSpeed /= abilityStats.attackSpeedMultiplier;
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() / 2f);
        weaponStats.SetDamage(weaponStats.GetDamage() / 2f);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() * 2f);
        
        // Stop the particle effect
        Destroy(frenzyEffect);
    }
}
