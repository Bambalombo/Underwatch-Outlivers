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

    private void Awake()
    {
        SetPlayerAbilityAndWeapon();
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
}

[System.Serializable]
public class PlayerClassAssets
{
    [SerializeField] public PlayerStatsController.PlayerClass className;
    [SerializeField] public GameObject ability;
    [SerializeField] public GameObject weapon;
}