using UnityEngine;

public class EnemyStatsController : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    //[SerializeField] private float armor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float lastAttackTime; // Time of the last attack
    [SerializeField] private float attackCooldown; // Time between attacks
    [SerializeField] private GameObject experienceDrop; // Experience drop prefab
    
    private void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
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
  
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    //public float GetArmor() => armor;
    public float GetMoveSpeed() => moveSpeed;
    public float GetDamage() => damage;
    public float GetLastAttackTime() => lastAttackTime;
    public float GetAttackCooldown() => attackCooldown;
    public GameObject GetExperienceDrop() => experienceDrop;
    
    
}