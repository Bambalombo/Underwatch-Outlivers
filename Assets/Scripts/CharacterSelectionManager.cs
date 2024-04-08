using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    //Individual character selection parents
    public GameObject P1CharacterSelectionParent;
    public GameObject P2CharacterSelectionParent;
    public GameObject P3CharacterSelectionParent;
    public GameObject P4CharacterSelectionParent;

    private List<int> selectedCharacters = new List<int>(); //Selected characters are stored as 0 = elementalist, 1 = voidwalker, 2 = mutant berserker, 3 = XI_017

    private int numberOfExpectedPlayers, charactersSaved;


    public void EnableCharacterSelectionUI(int numberOfPlayers)
    {
        numberOfExpectedPlayers = numberOfPlayers;

        P1CharacterSelectionParent.SetActive(numberOfPlayers >= 1);
        P2CharacterSelectionParent.SetActive(numberOfPlayers >= 2);
        P3CharacterSelectionParent.SetActive(numberOfPlayers >= 3);
        P4CharacterSelectionParent.SetActive(numberOfPlayers >= 4);

        // Initialize or clear the list whenever character selection UI is enabled
        selectedCharacters = new List<int>(new int[numberOfPlayers]);
    }

    public void SaveSelectedCharacter(int selectedCharacterIndex, int playerNumber)
    {
        selectedCharacters[playerNumber - 1] = selectedCharacterIndex;

        charactersSaved++;
        if (charactersSaved >= numberOfExpectedPlayers)
        {
            Debug.Log("Characters saved, proceeding to load level...");
            GameManager.SaveCharacterSelectionsAndLoadLevel(selectedCharacters);
        }
    }
}
    
    