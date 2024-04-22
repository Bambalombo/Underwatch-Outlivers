using UnityEngine;

[System.Serializable]
public class Talent {
    public string name;
    public string description;
    public int level, maxLevel; // Current level of the talent.
    public GameObject talentPrefab;
    public Sprite talentIcon;

    public Talent(string name, string description) {
        this.name = name;
        this.description = description;
        
    }
    
    // Method to apply the talent effect to a player
    public void ApplyEffectToPlayer(GameObject player) {
        ITalentEffect effect = talentPrefab.GetComponent<ITalentEffect>();
        if (effect != null) {
            effect.ApplyEffect(player);
        } else {
            Debug.LogWarning("Talent prefab does not contain an ITalentEffect implementation.");
        }
    }
}
