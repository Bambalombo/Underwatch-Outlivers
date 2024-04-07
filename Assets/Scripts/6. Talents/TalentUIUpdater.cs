using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentUIUpdater : MonoBehaviour
{
    // Inside TalentUIUpdater script
    public void UpdateTalentDisplays(GameObject[] talentOptionUIs, Talent[] talents)
    {
        for (int i = 0; i < talentOptionUIs.Length; i++)
        {
            TMP_Text nameText = talentOptionUIs[i].transform.Find("Name_text").GetComponent<TMP_Text>();
            TMP_Text descriptionText = talentOptionUIs[i].transform.Find("Description_text").GetComponent<TMP_Text>();
            
            nameText.text = talents[i].name; // Make sure talents[i].name is not null
            descriptionText.text = talents[i].description; // Use .text instead of SetText and make sure talents[i].description is not null
        }
    }

}