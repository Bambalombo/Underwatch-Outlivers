using UnityEngine;
using UnityEngine.Serialization;

public class WeaponStats : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage; // The damage of the weapon
    [SerializeField] private float attackCooldown; // The time between attacks
    [SerializeField] private float attackRange; // The range of the weapon
    [SerializeField] private float projectileSpeed; // The speed of the projectile
    [SerializeField] private float attackLifetime; // The lifetime of the bullet
    [SerializeField] private float knockback; // The knockback of the weapon
    [SerializeField] private float lifeStealAmount;

    public void SetDamage(float value) { damage = value; }
    public float GetDamage() { return damage; }

    public void SetAttackCooldown(float value) { attackCooldown = value; }
    public float GetAttackCooldown() { return attackCooldown; }

    public void SetAttackRange(float value) { attackRange = value; }
    public float GetAttackRange() { return attackRange; }

    public void SetProjectileSpeed(float value) { projectileSpeed = value; }
    public float GetProjectileSpeed() { return projectileSpeed; }

    public void SetAttackLifetime(float value) { attackLifetime = value; }
    public float GetAttackLifetime() { return attackLifetime; }
    
    public void SetKnockback(float value) { knockback = value; }
    public float GetKnockback() { return knockback; }
    
    public void SetLifeStealAmount(float value) { lifeStealAmount = value; }
    public float GetLifeStealAmount() { return lifeStealAmount; }
}
