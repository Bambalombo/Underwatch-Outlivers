using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable level;

    private void FixedUpdate()
    {
        UpdateExperienceUI(experience.value);
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

    private void UpdateExperienceUI(int exp)
    {
        experienceText.text = "Exp: " + exp;
    }

    private void UpdateLevelUI(int lvl)
    {
        levelText.text = "Lvl: " + lvl;
    }
}