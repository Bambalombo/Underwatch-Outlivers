using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTalents : MonoBehaviour
{
    //This script is located on each player character, and has two purposes for now:
    //1. Filter the possible talents the player can pick from based on their weapon and ability
    //2. Store their already selected talents

    private List<Talent> playerTalentPool; //Variable meant to hold the possible talents the player can get based on weapon/ability combiniation

    public PlayerStatsController playerStatsController; //We need this because the refernece to the currently selected class is in here

    public List<Talent> elementalistTalents, voidwalkerTalents, mutantberserkerTalents, XI_017talents;
    

    public List<Talent> InitializeUniqueTalentSet()
    {
        // TODO: Implement logic to filter talents based on the player's weapon and ability
        switch (playerStatsController.playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                playerTalentPool = elementalistTalents;
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                playerTalentPool = voidwalkerTalents;
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                playerTalentPool = mutantberserkerTalents;
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                playerTalentPool = XI_017talents;
                break;
        }

        return playerTalentPool;
    }

    public List<Talent> GetThreeRandomTalents(List<Talent> allTalents)
    {   
        List<Talent> selectedTalents = new List<Talent>();
        playerTalentPool = allTalents;

        // Ensure there are at least three talents to choose from
        if (playerTalentPool.Count <= 3)
        {
            return new List<Talent>(playerTalentPool); // Return a copy of the pool if it's 3 or fewer
        }
        else
        {
            // Create a list of indices to avoid selecting the same talent multiple times
            List<int> availableIndices = Enumerable.Range(0, playerTalentPool.Count).ToList();
            System.Random rand = new System.Random();

            for (int i = 0; i < 3; i++) // Select three unique talents
            {
                int randomIndex = rand.Next(availableIndices.Count);
                selectedTalents.Add(playerTalentPool[availableIndices[randomIndex]]);
                availableIndices.RemoveAt(randomIndex); // Remove the selected index to avoid duplicates
            }
        }

        return selectedTalents;
    }
}