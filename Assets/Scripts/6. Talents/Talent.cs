using UnityEngine;

[System.Serializable]
public class Talent {
    public string name;
    public string description;
    public int level; // Current level of the talent.
    public GameObject talentPrefab;

    public Talent(string name, string description) {
        this.name = name;
        this.description = description;
        this.level = 0;
    }
}