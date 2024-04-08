using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.UI;

public class IndividualCharacterSelectionBehavior : MonoBehaviour
{
    public int playerNumber; //Kinda the only way i can think of telling the manager which player slected who
    
    public Button GoLeftButton, GoRightButton, ConfirmButton;
    public TextMeshProUGUI WeaponDescriptionText, AbilityDescriptionText, CharacterNameText;
    public Image CharacterSprite;

    public GameObject doneSelectingOverlay;
    public GameObject selectingOverlay;

    public String elementalBolts,
        voidBlast,
        meleeSlash,
        droneWeapon,
        lightningBolt,
        abyssalRift,
        frenziedMutation,
        linusVæg;

    public Sprite[] characterSprites; // Array of character sprites
    private int currentSpriteIndex = 0; // Currently selected sprite index

    public CharacterSelectionManager CharacterSelectionManager;

    private void OnEnable()
    {
        doneSelectingOverlay.SetActive(false);
    }

    public void GoLeft()
    {
        if (characterSprites == null || characterSprites.Length == 0) return;

        currentSpriteIndex--;
        Debug.Log($"Before boundary check - Going Left, Current sprite index: {currentSpriteIndex}");

        if (currentSpriteIndex < 0)
        {   
            currentSpriteIndex = characterSprites.Length - 1; // Cycle back to last sprite
        }
        Debug.Log($"After boundary check - Going Left, Current sprite index: {currentSpriteIndex}");

        UpdateCharacterSelectionUI();
    }

    public void GoRight()
    {
        if (characterSprites == null || characterSprites.Length == 0) return;

        currentSpriteIndex++;
        Debug.Log($"Before boundary check - Going Right, Current sprite index: {currentSpriteIndex}");

        if (currentSpriteIndex >= characterSprites.Length)
        {
            currentSpriteIndex = 0; // Cycle back to first sprite
        }
        Debug.Log($"After boundary check - Going Right, Current sprite index: {currentSpriteIndex}");

        UpdateCharacterSelectionUI();
    }


    private void UpdateCharacterSelectionUI()
    {
        CharacterSprite.sprite = characterSprites[currentSpriteIndex];
        Debug.Log($"Changing Sprite to {characterSprites[currentSpriteIndex].name}");
        switch (currentSpriteIndex)
        {
            case 0:
                CharacterNameText.text = "Elementalist";
                WeaponDescriptionText.text = elementalBolts;
                AbilityDescriptionText.text = lightningBolt;
                break;
            case 1:
                CharacterNameText.text = "Void Walker";
                WeaponDescriptionText.text = voidBlast;
                AbilityDescriptionText.text = abyssalRift;
                break;
            case 2:
                CharacterNameText.text = "Mutant Berserker";
                WeaponDescriptionText.text = meleeSlash;
                AbilityDescriptionText.text = frenziedMutation;
                break;
            case 3:
                CharacterNameText.text = "XI_017";
                WeaponDescriptionText.text = droneWeapon;
                AbilityDescriptionText.text = linusVæg;
                break;
        }

    }

    public void ConfirmSelection()
    {
        ConfirmCharacterSelection(currentSpriteIndex);
    }

    private void ConfirmCharacterSelection(int selectedCharacterIndex)
    {
        CharacterSelectionManager.SaveSelectedCharacter(selectedCharacterIndex, playerNumber);
        doneSelectingOverlay.SetActive(true);
        selectingOverlay.SetActive(false);
    }
}
