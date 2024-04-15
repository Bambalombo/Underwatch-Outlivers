using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class AbilityStats : MonoBehaviour, IStatController
{
    [Header("Ability Stats")] [SerializeField]
    private float damage; // The damage of the ability
    [SerializeField] private float cooldown, // The cooldown of the ability
        range, // The range of the ability
        projectileSpeed, // The speed of the projectile
        lifetime, // The lifetime of the bullet or projectile
        knockback, // The knockback of the ability
        targetCount; // The amount of targets which the ability can hit

    public void SetDamage(float value) { damage = value; }
    public float GetDamage() { return damage; }

    public void SetAttackCooldown(float value) { cooldown = value; }
    public float GetAttackCooldown() { return cooldown; }

    public void SetAttackRange(float value) { range = value; }
    public float GetAttackRange() { return range; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }
    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetAttackLifetime(float value) { lifetime = value; }
    public float GetAttackLifetime() { return lifetime; }
    
    public void SetKnockback(float value) { knockback = value; }
    public float GetKnockback() { return knockback; }
    
    public void SetTargetCount(float value) { targetCount = value; }
    public float GetTargetCount() { return targetCount; }
}
