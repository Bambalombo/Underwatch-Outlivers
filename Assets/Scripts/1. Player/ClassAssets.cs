using System;
using System.Collections.Generic;
using UnityEngine;

public class ClassAssets : MonoBehaviour
{
    [Header("Assignments")]
    [SerializeField] private GameObject abilityParent; // The parent of the abilities
    [SerializeField] private GameObject weaponParent; // The parent of the weapons
    [SerializeField] private PlayerStatsController playerStatsController;
    
    [Header("Class Assets")]
    [SerializeField] private PlayerClassAssets[] classAssets;

    [Header("Currently Active Ability")]
    [SerializeField] private List<GameObject> activeAbilities; // List of abilities the player has
    
    [Header("Currently Active Weapon")]
    [SerializeField] private List<GameObject> activeWeapons; // List of weapons the player has

    [Header("Player Collider")]
    private CapsuleCollider2D playerCollider;
    [SerializeField] private CapsuleCollider2D playerHitbox;

    private void Awake()
    {
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    private void SetPlayerAbilityAndWeapon()
    {
        activeAbilities = new List<GameObject>();

        GameObject ability = null;
        GameObject weapon = null;
        var playerClass = playerStatsController.playerClass;

        foreach (var classAsset in classAssets)
        {
            if (classAsset.className == playerClass)
            {
                //Debug.Log("Player class: " + playerClass);
                
                ability = Instantiate(classAsset.ability, parent: abilityParent.transform);
                weapon = Instantiate(classAsset.weapon, parent: weaponParent.transform);
                break;
            }
        }
        if (ability)
            activeAbilities.Add(ability);
        if (weapon)
            activeWeapons.Add(weapon);
    }
    
    // Sets the correct hitbox for the player based of which character they are
    private void SetPlayerHitbox()
    {
        var playerClass = playerStatsController.playerClass;
        
        // Set the hitbox size and offset based on the player class
        foreach (var classAsset in classAssets)
        {
            if (classAsset.className == playerClass)
            {
                playerHitbox.offset = classAsset.hitboxOffset;
                playerCollider.offset = classAsset.hitboxOffset;
                
                playerHitbox.size = classAsset.hitboxSize;
                playerCollider.size = classAsset.hitboxSize;
                //Debug.Log("Player class: " + playerClass);
                //Debug.Log("Hitbox offset: " + playerHitbox.offset + " Hitbox size: " + playerHitbox.size);
                
                playerHitbox.direction = classAsset.hitboxDirection == PlayerClassAssets.HitboxDirection.Vertical
                    ? CapsuleDirection2D.Vertical
                    : CapsuleDirection2D.Horizontal;
                playerCollider.direction = classAsset.hitboxDirection == PlayerClassAssets.HitboxDirection.Vertical
                    ? CapsuleDirection2D.Vertical
                    : CapsuleDirection2D.Horizontal;
                break;
            }
        }
    }
    
    public GameObject GetActiveWeapons()
    {
        return activeWeapons[0];
    }

    public GameObject GetActiveAbilities()
    {
        return activeAbilities[0];
    }
    
    public void ChangeClass(int classIndex)
    {
        playerStatsController.ChangePlayerClass(classIndex);
        foreach (var ability in activeAbilities)
        {
            Destroy(ability);
        }
        foreach (var weapon in activeWeapons)
        {
            Destroy(weapon);
        }
        activeAbilities.Clear();
        activeWeapons.Clear();
        SetPlayerAbilityAndWeapon();
        SetPlayerHitbox();
    }
}

[System.Serializable]
public class PlayerClassAssets
{
    [SerializeField] public PlayerStatsController.PlayerClass className;
    [SerializeField] public GameObject ability;
    [SerializeField] public GameObject weapon;
    [Header("Hitbox Settings")]
    [SerializeField] public Vector2 hitboxOffset;
    [SerializeField] public Vector2 hitboxSize;
    public enum HitboxDirection
    {
        Vertical,
        Horizontal
    }
    [SerializeField] public HitboxDirection hitboxDirection;
}

