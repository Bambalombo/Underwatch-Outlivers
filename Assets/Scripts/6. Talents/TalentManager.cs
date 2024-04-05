using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    public List<Talent> allTalents; // Populate this list in the Unity Editor
    public TalentUI[] talentUIElements; // Assign your TalentUI elements here in the Unity Editor
    private GameObject[] playerGameobjects;

    void Start() {
        // Find the GameManager
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerGameobjects = GameManager.GetPlayerGameObjects();
        
    }

    public void OpenTalentMenu(int newLevel)
    {
        Debug.Log("OPEN TALENT MENU");
        TogglePlayerCanvases(true); // Enable the canvases
        _gameManager.TogglePause();
    }

    public void TalentSelected(Talent selectedTalent) {
        Debug.Log("CLOSE TALENT MENU");
        TogglePlayerCanvases(false); // Disable the canvases
        _gameManager.TogglePause();
    }

    private void TogglePlayerCanvases(bool isActive)
    {
        foreach (GameObject player in playerGameobjects)
        {
            // Find the "InteractableCanvas" within each player and toggle its active state
            foreach (Transform child in player.transform)
            {
                if (child.name == "InteractableCanvas")
                {
                    child.gameObject.SetActive(isActive);
                    break; // Found the canvas, no need to search further
                }
            }
        }
    }
    

}
