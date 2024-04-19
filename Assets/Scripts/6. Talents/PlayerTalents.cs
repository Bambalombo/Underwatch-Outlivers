using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTalents : MonoBehaviour
{
    //This script is located on each player character, and has two purposes for now:
    //1. Filter the possible talents the player can pick from based on their weapon and ability
    //2. Store their already selected talents

    private List<Talent> commonTalentPool; //Variable meant to hold all the basic stat upgrade talents
    private List<Talent> rareTalentPool; //Variable meant to hold the more unique talents for each weapon/ability

    public PlayerStatsController playerStatsController; //We need this because the refernece to the currently selected class is in here

    public List<Talent> commonElementalistTalents,
        commonVoidwalkerTalents,
        commonMutantberserkerTalents,
        commonXI_017Talents,
        rareElementalistTalents,
        rareVoidwalkerTalents,
        rareMutantberserkerTalents,
        rareXI_017Talents;


    public (List<Talent> commonTalentPool, List<Talent> rareTalentPool) InitializeUniqueTalentSet()
    {
        List<Talent> commonTalentPool = null;
        List<Talent> rareTalentPool = null;

        switch (playerStatsController.playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                commonTalentPool = commonElementalistTalents;
                rareTalentPool = rareElementalistTalents;
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                commonTalentPool = commonVoidwalkerTalents;
                rareTalentPool = rareVoidwalkerTalents;
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                commonTalentPool = commonMutantberserkerTalents;
                rareTalentPool = rareMutantberserkerTalents;
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                commonTalentPool = commonXI_017Talents;
                rareTalentPool = rareXI_017Talents;
                break;
        }

        return (commonTalentPool, rareTalentPool); //Dum workaround til at returne mere end 1 variable og nej jeg gidder ikk returne et array >:(
    }


    public List<Talent> GetThreeRandomTalents(List<Talent> allTalents)
    {   
        List<Talent> selectedTalents = new List<Talent>();
        commonTalentPool = allTalents;

        // Ensure there are at least three talents to choose from
        if (commonTalentPool.Count <= 3)
        {
            return new List<Talent>(commonTalentPool); // Return a copy of the pool if it's 3 or fewer
        }
        else
        {
            // Create a list of indices to avoid selecting the same talent multiple times
            List<int> availableIndices = Enumerable.Range(0, commonTalentPool.Count).ToList();
            System.Random rand = new System.Random();

            for (int i = 0; i < 3; i++) // Select three unique talents
            {
                int randomIndex = rand.Next(availableIndices.Count);
                selectedTalents.Add(commonTalentPool[availableIndices[randomIndex]]);
                availableIndices.RemoveAt(randomIndex); // Remove the selected index to avoid duplicates
            }
        }

        return selectedTalents;
    }
}
