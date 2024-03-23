using UnityEngine;
using TMPro;

public class TalentUI : MonoBehaviour {
    public TextMeshProUGUI talentNameText;
    public TextMeshProUGUI talentDescriptionText;
    // Reference to TalentManager to notify about talent selection
    public TalentManager talentManager;

    // The talent this UI represents
    private Talent currentTalent;

    public void SetTalent(Talent talent) {
        currentTalent = talent; // Store the current talent for reference
        talentNameText.text = talent.name;
        talentDescriptionText.text = talent.description;
    }

    public void OnTalentSelected() {
        Debug.Log("Talent selected!");
        talentManager.TalentSelected(currentTalent);
    }

}