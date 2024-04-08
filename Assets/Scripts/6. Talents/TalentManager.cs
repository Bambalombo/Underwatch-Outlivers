using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    //TODO randomize talents + lave et tjek for at de stat talents der kommer frem rent faktisk giver mening for playercharacteren (f.eks giver ability dmg ikk mening for wall ability)
    //Maybe some sort of checker that checks the weapon and ability, and based on what it finds it adds a predefined set of talents
    
    public List<Talent> allTalents; // Populate this list in the Unity Editor
    [SerializeField] private GameObject[] playerGameObjects;

    void Start() {
        playerGameObjects = GameManager.GetPlayerGameObjects();
        
    }

    public void OpenTalentMenu(int newLevel)
    {
        TogglePlayerCanvases(true); // Enable the canvases
        GameManager.TogglePause();
    }

    public void TalentSelected(Talent selectedTalent) {
        TogglePlayerCanvases(false); // Disable the canvases
        GameManager.TogglePause();
    }

    private void TogglePlayerCanvases(bool isActive)
    {
        foreach (GameObject player in playerGameObjects)
        {
            Transform interactableCanvas = null;
        
            // Find the "InteractableCanvas" within each player and toggle its active state
            foreach (Transform child in player.transform)
            {
                if (child.name == "InteractableCanvas")
                {
                    child.gameObject.SetActive(isActive);
                    interactableCanvas = child; // Store the reference to interactableCanvas
                    break; // Found the canvas, no need to search further
                }
            }

            // If the canvas is being activated, populate and update the talent options
            if (isActive && interactableCanvas != null)
            {
                PopulateTalentOptions(interactableCanvas);
            }
        }
    }

    private void PopulateTalentOptions(Transform interactableCanvas)
    {
        // Access the PlayerTalents script attached to the player (assumes interactableCanvas is a child of the player GameObject)
        PlayerTalents playerTalents = interactableCanvas.GetComponentInParent<PlayerTalents>();
        if (playerTalents != null)
        {
            // Get three random talents applicable to this players weapon/ability combination
            List<Talent> randomTalents = playerTalents.GetThreeRandomTalents(allTalents);
            // Call UpdateTalentUI to update the talent UI panel for this player
            UpdateTalentUI(interactableCanvas, randomTalents);
        }
        else
        {
            Debug.LogWarning("PlayerTalents script not found on player GameObject", interactableCanvas.parent.gameObject);
        }
    }
    
    private void UpdateTalentUI(Transform interactableCanvas, List<Talent> randomTalents)
    {
        if (randomTalents == null || randomTalents.Count == 0)
        {
            Debug.LogError("No talents provided for updating the UI.");
            return;
        }

        // The TalentPanel is inside TalentManager which is a sibling of InteractableCanvas, not a child
        // So we should find TalentManager in the parent of InteractableCanvas
        Transform talentManagerTransform = interactableCanvas.Find("TalentManager/TalentPanel/TalentBackground");
        if (talentManagerTransform == null)
        {
            Debug.LogError("TalentBackground not found.");
            return;
        }

        // Now we have the correct parent object, and we can find the buttons under TalentBackground
        TalentUIUpdater talentUIUpdater = interactableCanvas.GetComponentInChildren<TalentUIUpdater>(true);
        if (talentUIUpdater == null)
        {
            Debug.LogError("TalentUIUpdater component not found in children of TalentBackground.");
            return;
        }

        for (int i = 0; i < randomTalents.Count; i++)
        {
            string buttonName = $"Button{i+1}";
            Transform buttonTransform = talentManagerTransform.Find(buttonName);
            if (buttonTransform == null)
            {
                Debug.LogError($"{buttonName} not found under TalentBackground.");
                continue; // Skip this iteration and move to the next one
            }

            GameObject talentOptionUI = buttonTransform.gameObject;
            if (talentOptionUI == null)
            {
                Debug.LogError($"Talent option UI game object for {buttonName} is missing.");
                continue;
            }

            // If everything is found, proceed with updating the UI
            talentUIUpdater.UpdateTalentDisplays(new GameObject[] { talentOptionUI }, new Talent[] { randomTalents[i] });
        }
    }




    
    
    

}
