using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    //TODO randomize talents + lave et tjek for at de stat talents der kommer frem rent faktisk giver mening for playercharacteren (f.eks giver ability dmg ikk mening for wall ability)
    //Maybe some sort of checker that checks the weapon and ability, and based on what it finds it adds a predefined set of talents

    private List<Talent> _playerTalentPool;
    [SerializeField] private GameObject[] playerGameObjects;
    private GameManager _gameManager;

    private int _talentsPicked;
    
    [SerializeField] private GameObject talentBackground;

    void Start() {
        playerGameObjects = GameManager.GetPlayerGameObjects();
        // Find the GameManager
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        talentBackground.SetActive(false);
    }

    public void OpenTalentMenu(int newLevel)
    {
        if (talentBackground.activeSelf == false)
        {
            //Debug.Log("Talent background is not active, activating it now");
            talentBackground.SetActive(true);
        }
        
        TogglePlayerCanvases(true); // Enable the canvases
        _gameManager.TogglePause();
        
        _talentsPicked = 0;
    }

    public void TalentSelected(Talent selectedTalent, GameObject playerGameobject) {
        selectedTalent.ApplyEffectToPlayer(playerGameobject);
        _talentsPicked++;
        if (_talentsPicked >= playerGameObjects.Length)
        {
            if (talentBackground.activeSelf)
            {
                //Debug.Log("Talent background is active, deactivating it now");
                talentBackground.SetActive(false);
            }
            
            Debug.Log("Enough talents picked, time to UNPAUSE");
            _gameManager.TogglePause();
        }
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
            _playerTalentPool = playerTalents.InitializeUniqueTalentSet(); //lidt dumt den initializer talent pool hver gang men jeg er speedy gonzales så ses
            List<Talent> randomTalents = playerTalents.GetThreeRandomTalents(_playerTalentPool);
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
            
            talentOptionUI.GetComponent<TalentUI>().SetTalent(randomTalents[i]);
        }
    }




    
    
    

}
