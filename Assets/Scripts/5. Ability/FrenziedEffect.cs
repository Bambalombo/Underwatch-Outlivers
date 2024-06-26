using System;
using System.Collections;
using UnityEngine;

public class FrenziedEffect : MonoBehaviour
{
    private float duration;
    private float _healthDrainRate, _statBuffMultiplier;

    private bool buffActive;
    
    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private WeaponStats weaponStats;
    
    public void Initialize(GameObject playerGameObject, float healthDrain, float buffMultiplier, float abilityDuration)
    {
        _healthDrainRate = healthDrain;
        _statBuffMultiplier = buffMultiplier;
        duration = abilityDuration;
        playerStatsController = playerGameObject.GetComponent<PlayerStatsController>();
        playerHealthController = playerGameObject.GetComponent<PlayerHealthController>();
        weaponStats = playerGameObject.GetComponentInChildren<WeaponStats>();

        // Apply buff
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() * _statBuffMultiplier);
        weaponStats.SetDamage(weaponStats.GetDamage() * _statBuffMultiplier);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() / _statBuffMultiplier);
        buffActive = true;


        StartCoroutine(EffectCoroutine(playerStatsController, playerHealthController, weaponStats));
    }

    private IEnumerator EffectCoroutine(PlayerStatsController playerStatsController, PlayerHealthController playerHealthController, WeaponStats weaponStats)
    {
        float remainingDuration = duration;

        while (remainingDuration > 0)
        {
            playerHealthController.PlayerTakeDamage(_healthDrainRate * Time.deltaTime);
            remainingDuration -= Time.deltaTime;
            yield return null;
        }

        if (buffActive)
        {
            DisableBuff();
            buffActive = false;
        }

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (buffActive)
        {
            DisableBuff();
            buffActive = false;
        }

        StopCoroutine(EffectCoroutine(playerStatsController, playerHealthController, weaponStats));
        
        Destroy(gameObject);
    }

    private void DisableBuff()
    {
        Debug.Log("Disabling buff");
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() / _statBuffMultiplier);
        weaponStats.SetDamage(weaponStats.GetDamage() / _statBuffMultiplier);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() * _statBuffMultiplier);
    }
}