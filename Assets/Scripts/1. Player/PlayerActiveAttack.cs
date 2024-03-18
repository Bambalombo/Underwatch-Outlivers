using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveAttack : MonoBehaviour
{
    [Header("Assignments")]
    [SerializeField] private GameObject abilityParent; // The parent of the abilities
    [SerializeField] private GameObject weaponParent; // The parent of the weapons
    [SerializeField] private PlayerStatsController playerStatsController;
    
    [Header("Default Class Abilities")]
    [SerializeField] private GameObject elementalistAbility;
    [SerializeField] private GameObject voidwalkerAbility;
    [SerializeField] private GameObject mutantBerserkerAbility;
    [SerializeField] private GameObject xenobiologistAbility;
    [SerializeField] private GameObject xi017Ability;
    
    [Header("Default Class Weapons")]
    [SerializeField] private GameObject elementalistWeapon;
    [SerializeField] private GameObject voidwalkerWeapon;
    [SerializeField] private GameObject mutantBerserkerWeapon;
    [SerializeField] private GameObject xenobiologistWeapon;
    [SerializeField] private GameObject xi017Weapon;

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
        switch (playerStatsController.playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                ability = Instantiate(elementalistAbility, parent: abilityParent.transform);
                weapon = Instantiate(elementalistWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                ability = Instantiate(voidwalkerAbility, parent: abilityParent.transform);
                weapon = Instantiate(voidwalkerWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                ability = Instantiate(mutantBerserkerAbility, parent: abilityParent.transform);
                weapon = Instantiate(mutantBerserkerWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Xenobiologist:
                ability = Instantiate(xenobiologistAbility, parent: abilityParent.transform);
                weapon = Instantiate(xenobiologistWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                if (xi017Ability != null)
                    ability = Instantiate(xi017Ability, parent: abilityParent.transform);
                else
                    Debug.LogWarning("XI_017 Ability is not assigned in the inspector");

                if (xi017Weapon != null)
                    weapon = Instantiate(xi017Weapon, parent: weaponParent.transform);
                else
                    Debug.LogWarning("XI_017 Weapon is not assigned in the inspector");
                break;
            default:
                Debug.LogWarning("Class not detected. No ability or weapon was assigned.");
                break;
        }
        if (ability)
            activeAbilities.Add(ability);
        if (weapon)
            activeWeapons.Add(weapon);
    }
}
