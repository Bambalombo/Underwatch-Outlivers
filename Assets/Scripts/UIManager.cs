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

    private void FixedUpdate()
    {
        UpdateExperienceUI(experience.value, nextLevelExperience.value);
        UpdateLevelUI(level.value);
    }

    void Awake()
    {
        /*if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }*/
    }

    private void UpdateExperienceUI(int exp, int nextLevelExp)
    {
        experienceText.text = "Exp: " + exp + " / " + nextLevelExp;
    }

    private void UpdateLevelUI(int lvl)
    {
        levelText.text = "Lvl: " + lvl;
    }
}