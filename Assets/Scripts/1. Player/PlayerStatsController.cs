using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    //[SerializeField] private float armor;
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float damage; // Should be changed by weapon?
    [SerializeField] private float attackSpeed;
    //[SerializeField] private float attackRange; //NOT USED YET
    //[SerializeField] private float attackCooldown; 
    [SerializeField] private float experiencePickupRange; // The range in which the player can pick up experience
    //[SerializeField] private float levelScalingFactor; // The level scaling factor for the experience needed to level up
    //[SerializeField] private int experience; // Player experience
    //[SerializeField] private int baseExperience; // Experience needed to reach level 2
    //[SerializeField] private int experienceToNextLevel; // Experience needed to reach next level
    //[SerializeField] private int level; // Player level
    [SerializeField] private Vector3 playerPosition; // Player position

    
    private void Start()
    {
        currentHealth = maxHealth;
        
        //experience = 0;
        //level = 1;
        //experienceToNextLevel = baseExperience;
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
    /*public void SetDamage(float value)
    {
        damage = value;
    }*/
    public void SetAttackSpeed(float value)
    {
        attackSpeed = value;
    }
    /*public void SetAttackRange(float value)
    {
        attackRange = value;
    }
    public void SetAttackCooldown(float value)
    {
        attackCooldown = value;
    }*/
    public void SetExperiencePickupRange(float value)
    {
        experiencePickupRange = value;
    }
    /*public void SetLevelScalingFactor(float value)
    {
        levelScalingFactor = value;
    }
    public void SetExperience(int value)
    {
        experience = value;
    }
    public void SetBaseExperience(int value)
    {
        baseExperience = value;
    }
    public void SetExperienceToNextLevel(int value)
    {
        experienceToNextLevel = value;
    }
    public void SetLevel(int value)
    {
        level = value;
    }*/
    public void SetPlayerPosition(Vector3 value)
    {
        playerPosition = value;
    }
    
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    //public float GetArmor() => armor;
    public float GetMoveSpeed() => moveSpeed;
    //public float GetDamage() => damage;

    public float GetAttackSpeed() => attackSpeed;
    //public float GetAttackRange() => attackRange;
    //public float GetAttackCooldown() => attackCooldown;
    public float GetExperiencePickupRange() => experiencePickupRange;
    /*public float GetLevelScalingFactor() => levelScalingFactor;
    public int GetExperience() => experience;
    public int GetBaseExperience() => baseExperience;
    public int GetExperienceToNextLevel() => experienceToNextLevel;
    public int GetLevel() => level;*/
    public Vector3 GetPlayerPosition() => playerPosition;
    
    
    
}