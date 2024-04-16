using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable nextLevelExperience;
    [SerializeField] private IntVariable level;

    private void Update()
    {
        UpdateExpUI();
    }

    private void UpdateExperienceUI(int exp, int nextLevelExp)
    {
        experienceText.text = "Exp: " + exp + " / " + nextLevelExp;
    }

    private void UpdateLevelUI(int lvl)
    {
        levelText.text = "Lvl: " + lvl;
    }

    public void UpdateExpUI()
    {
        UpdateExperienceUI(experience.value, nextLevelExperience.value);
        UpdateLevelUI(level.value);
    }
}