using System;
using System.Collections;
using UnityEngine;

public class FrenziedEffect : MonoBehaviour
{
    [SerializeField] private float duration = 10f;
    private float _healthDrainRate, _statBuffMultiplier;

    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private WeaponStats weaponStats;
    
    public void Initialize(GameObject playerGameObject, float healthDrain, float buffMultiplier)
    {
        _healthDrainRate = healthDrain;
        _statBuffMultiplier = buffMultiplier;
        playerStatsController = playerGameObject.GetComponent<PlayerStatsController>();
        playerHealthController = playerGameObject.GetComponent<PlayerHealthController>();
        weaponStats = playerGameObject.GetComponentInChildren<WeaponStats>();

        // Apply buff
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() * _statBuffMultiplier);
        weaponStats.SetDamage(weaponStats.GetDamage() * _statBuffMultiplier);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() / _statBuffMultiplier);



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

        // Revert buff
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() / _statBuffMultiplier);
        weaponStats.SetDamage(weaponStats.GetDamage() / _statBuffMultiplier);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() * _statBuffMultiplier);

        Destroy(gameObject);
    }

    private void OnDisable()
    {
        // Revert buff
        playerStatsController.SetMoveSpeed(playerStatsController.GetMoveSpeed() / _statBuffMultiplier);
        weaponStats.SetDamage(weaponStats.GetDamage() / _statBuffMultiplier);
        weaponStats.SetAttackCooldown(weaponStats.GetAttackCooldown() * _statBuffMultiplier);
        StopCoroutine(EffectCoroutine(playerStatsController, playerHealthController, weaponStats));
        
        Destroy(gameObject);
    }
}