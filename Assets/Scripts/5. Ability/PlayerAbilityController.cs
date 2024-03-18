using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour
{
    [Header("Assignments")]
    [SerializeField] private GameObject abilityParent; // The parent of the weapons
    [SerializeField] private PlayerStatsController playerStatsController;
    
    [Header("Default Class Abilities")]
    [SerializeField] private GameObject elementalistAbility;
    [SerializeField] private GameObject voidwalkerAbility;
    [SerializeField] private GameObject mutantBerserkerAbility;
    [SerializeField] private GameObject xenobiologistAbility;
    [SerializeField] private GameObject Xi017Ability;

    [Header("Currently Active Ability")]
    [SerializeField] private List<GameObject> activeAbilities; // List of weapons the player has

    private void Awake()
    {
        SetPlayerAbility();
    }

    private void SetPlayerAbility()
    {
        activeAbilities = new List<GameObject>();

        GameObject ability = null;
        switch (playerStatsController.playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                ability = Instantiate(elementalistAbility, parent: abilityParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                ability = Instantiate(voidwalkerAbility, parent: abilityParent.transform);
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                ability = Instantiate(mutantBerserkerAbility, parent: abilityParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Xenobiologist:
                ability = Instantiate(xenobiologistAbility, parent: abilityParent.transform);
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                ability = Instantiate(Xi017Ability, parent: abilityParent.transform);
                break;
            default:
                Debug.Log("Class not detected. No ability was assigned.");
                break;
        }
        if (ability)
            activeAbilities.Add(ability);
    }
}
