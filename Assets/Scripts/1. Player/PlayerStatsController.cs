using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private bool playerCanLooseHealth = true;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;
    [SerializeField] private float respawnTime = 15f;
    //[SerializeField] private float armor;
    [SerializeField] private float moveSpeed;
    //[SerializeField] private float damage; // Should be changed by weapon?
    //[SerializeField] private float attackSpeed; //I think this variable is called attackCooldown in WeaponStats
    //[SerializeField] private float attackRange; //NOT USED YET
    //[SerializeField] private float attackCooldown; 
    [SerializeField] private float experiencePickupRange; // The range in which the player can pick up experience
    //[SerializeField] private float levelScalingFactor; // The level scaling factor for the experience needed to level up
    //[SerializeField] private int experience; // Player experience
    //[SerializeField] private int baseExperience; // Experience needed to reach level 2
    //[SerializeField] private int experienceToNextLevel; // Experience needed to reach next level
    //[SerializeField] private int level; // Player level
    [SerializeField] private Vector3 playerPosition; // Player position

    [SerializeField] private Vector2 lastMoveDirection;
    // NOT SURE IF PLAYERS SHOULD HAVE INDIVIDUAL EXPERIENCE

    public PlayerHealthController playerHealthController;
    
    [Header("Class Attributes")]
    [SerializeField] public PlayerClass playerClass;
    public enum PlayerClass 
    {
        Elementalist,
        Voidwalker,
        MutantBerserker,
        XI_017,
        Xenobiologist
    }
    // Inside PlayerStatsController class   
    
    private void Start()
    {
        currentHealth = maxHealth;
        lastMoveDirection = new Vector2(1f, 0f);

        //experience = 0;
        //level = 1;
        //experienceToNextLevel = baseExperience;
    }
    
    public void SetCurrentHealth(float value)
    {
        if (value > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (value < 0)
        {
            currentHealth = playerCanLooseHealth ? 0 : maxHealth; // Set health to maxHealth when playerCanLooseHealth is false
        }
        else
        {
            currentHealth = playerCanLooseHealth ? value : maxHealth; // Set health to maxHealth when playerCanLooseHealth is false
        }
        
    }
    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        playerHealthController.PlayerTakeDamage(0); //Lige en hurtig cheat for at få den til at update UI, håber ikk det kommer til at fucke med noget senere:) 
    }
    public void SetRespawnTime(float value)
    {
        respawnTime = value;
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
    /*public void SetAttackSpeed(float value)
    {
        attackSpeed = value;
    }*/
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
    
    public void SetLastMoveDirection(Vector2 value)
    {
        lastMoveDirection = value;
    }
    
    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetRespawnTime() => respawnTime;

    //public float GetArmor() => armor;
    public float GetMoveSpeed() => moveSpeed;
    //public float GetDamage() => damage;

    //public float GetAttackSpeed() => attackSpeed;
    //public float GetAttackRange() => attackRange;
    //public float GetAttackCooldown() => attackCooldown;
    public float GetExperiencePickupRange() => experiencePickupRange;
    /*public float GetLevelScalingFactor() => levelScalingFactor;
    public int GetExperience() => experience;
    public int GetBaseExperience() => baseExperience;
    public int GetExperienceToNextLevel() => experienceToNextLevel;
    public int GetLevel() => level;*/
    public Vector3 GetPlayerPosition() => playerPosition;

    public Vector2 GetLastMoveDirection() => lastMoveDirection;

    public void ChangePlayerClass(int classIndex)
    {
        if (classIndex < 0 || classIndex >= System.Enum.GetValues(typeof(PlayerClass)).Length)
        {
            Debug.LogError("Invalid class index.");
            return;
        }

        playerClass = (PlayerClass)classIndex;
    }

}