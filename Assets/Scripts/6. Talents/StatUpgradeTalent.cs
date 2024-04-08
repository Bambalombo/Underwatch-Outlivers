using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StatUpgradeTalent : MonoBehaviour, ITalentEffect
{
    private PlayerStatsController _playerStatsController;
    private WeaponStats _weaponStats;
    private AbilityStats _abilityStats;
    
    private enum Stat
    {
        playerHealth,
        playerMovementSpeed,
        weaponDamage,
        weaponCooldown,
        weaponRange,
        weaponProjectileSpeed,
        weaponAttackLifetime,
        weaponKnockback,
        weaponLifesteal,
        abilityDamage,
        abilityCooldown,
        abilityRange,
        abilityProjectileSpeed,
        abilityLifetime,
        abilityKnockback
    }

    [Header("Stat Selection")]
    [SerializeField] private Stat selectedStat;
    
    [Header("Percent increase written as a decimal (for 5% write 0.05 e.g)")]
    //Amount skal bruges som en procent (Så f.eks 5% mere damage), så skriv det som et decimal tal (0.05 for 5%)
    public float amountPercent;
    

    public void ApplyEffect(GameObject player)
    {
        // Get the components only when needed based on the stat type
        string statName = selectedStat.ToString();
        if (statName.StartsWith("player"))
        {
            _playerStatsController = player.GetComponent<PlayerStatsController>();
            ApplyPlayerStat(statName, amountPercent);
        }
        else if (statName.StartsWith("weapon"))
        {
            _weaponStats = player.GetComponentInChildren<WeaponStats>(true); // 'true' to include inactive GameObjects
            ApplyWeaponStat(statName, amountPercent);
        }
        else if (statName.StartsWith("ability"))
        {
            _abilityStats = player.GetComponentInChildren<AbilityStats>(true); // 'true' to include inactive GameObjects
            ApplyAbilityStat(statName, amountPercent);
        }
        else
        {
            Debug.LogWarning("Stat category not recognized.");
        }
    }

    private void ApplyPlayerStat(string statName, float amount)
    {
        switch (statName)
        {
            case "playerHealth":
                _playerStatsController.SetMaxHealth(_playerStatsController.GetMaxHealth()*(1+amount));
                break;
            case "playerMovementSpeed":
                _playerStatsController.SetMoveSpeed(_playerStatsController.GetMoveSpeed()*(1+amount));
                break;
        }
    }

    private void ApplyWeaponStat(string statName, float amount)
    {
        switch (statName)
        {
            case "weaponDamage":
                _weaponStats.SetDamage(_weaponStats.GetDamage()*(1+amount));
                break;
            case "weaponCooldown":
                _weaponStats.SetAttackCooldown(_weaponStats.GetAttackCooldown() * (1-amount)); //OBS: Cooldown bliver minusset i stedet for så f.eks 5% CDR bliver til at abilityens cooldown = 95% af den originale
                break;
            case "weaponRange":
                Debug.Log("weaponRange not implemented/used yet");
                break;
            case "weaponProjectileSpeed":
                _weaponStats.SetProjectileSpeed(_weaponStats.GetProjectileSpeed() * (1 + amount));
                break;
            case "weaponAttackLifetime":
                Debug.Log("weaponLifeTime not implemented/used yet");
                break;
            case "weaponKnockback":
                Debug.Log("weaponKnockback not implemented/used yet");
                break;
            case "weaponLifesteal":
                _weaponStats.SetLifeStealAmount(_weaponStats.GetLifeStealAmount()*(1+amount));
                break;
        }
    }

    private void ApplyAbilityStat(string statName, float amount)
    {
        switch (statName)
        {
            case "abilityDamage":
                _abilityStats.SetDamage(_abilityStats.GetDamage()*(1+amount));
                break;
            case "abilityCooldown":
                _abilityStats.SetAttackCooldown(_abilityStats.GetAttackCooldown()*(1-amount)); //OBS: Cooldown bliver minusset i stedet for så f.eks 5% CDR bliver til at abilityens cooldown = 95% af den originale
                break;
            case "abilityRange":
                _abilityStats.SetAttackRange(_abilityStats.GetAttackRange()*(1+amount));
                break;
            case "abilityProjectileSpeed":
                _abilityStats.SetProjectileSpeed(_abilityStats.GetProjectileSpeed()*(1+amount));
                break;
            case "abilityLifetime":
                _abilityStats.SetAttackLifetime(_abilityStats.GetAttackLifetime()*(1+amount));
                break;
            case "abilityKnockback":
                _abilityStats.SetKnockback(_abilityStats.GetKnockback()*(1+amount)); 
                break;
                
        }
    }

}



