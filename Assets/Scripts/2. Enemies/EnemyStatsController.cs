using System;
using DefaultNamespace;
using UnityEngine;

public class EnemyStatsController : MonoBehaviour, IStatController
{
    [SerializeField] private bool isBoss; // If the enemy is a boss
    [SerializeField] private bool isFoundByPlayer; // If the enemy has been found by the player (Used for bosses)
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    //[SerializeField] private float armor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lastAttackTime; // Time of the last attack
    [SerializeField] private float attackCooldown; // Time between attacks
    [SerializeField] private GameObject experienceDrop; // Experience drop prefab
    [SerializeField] private GameObject healthPickup; // Health pickup prefab
    [SerializeField] private float healthDropChance; // Chance for health drop (0-100)

    private void Awake()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
        
        // If the enemy is a boss, it has not been found by the player
        isFoundByPlayer = !isBoss;

        if (!isFoundByPlayer)
            GetComponent<EnemyTeleportToPlayer>().enabled = false;
    }
    
    public void SetCurrentHealth(float value)
    {
        currentHealth = value;
    }
    public void SetMaxHealth(float value)
    {
        maxHealth = value;
    }
    /*public void SetArmor(float value)
    {
        armor = value;
    }*/
    public void SetMoveSpeed(float value)
    {
        moveSpeed = value;
    }
    public void SetDamage(float value)
    {
        damage = value;
    }
    public void SetLastAttackTime(float value)
    {
        lastAttackTime = value;
    }
    public void SetAttackCooldown(float value)
    {
        attackCooldown = value;
    }
    public void SetIsFoundByPlayer(bool value)
    {
        isFoundByPlayer = value;
    }
  
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    //public float GetArmor() => armor;
    public float GetMoveSpeed() => moveSpeed;
    public float GetDamage() => damage;
    public float GetLastAttackTime() => lastAttackTime;
    public float GetAttackCooldown() => attackCooldown;
    public GameObject GetExperienceDrop() => experienceDrop;
    public GameObject GetHealthPickup() => healthPickup;
    public float GetHealthDropChance() => healthDropChance;
    public bool GetIsFoundByPlayer() => isFoundByPlayer;
    public bool GetIsBoss() => isBoss;
    
    
}
