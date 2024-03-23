[System.Serializable]
public class Talent {
    public string name;
    public string description;
    public int level; // Current level of the talent.
    // Add more properties as needed, such as effects, cooldown, etc.

    public Talent(string name, string description) {
        this.name = name;
        this.description = description;
        this.level = 0;
    }
}