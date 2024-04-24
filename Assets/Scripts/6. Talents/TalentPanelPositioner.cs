using UnityEngine;
using UnityEngine.UI; 

public class TalentPanelPositioner : MonoBehaviour
{
    //THIS SCRIPT IS PURELY MEANT TO HANDLE THE POSITIONS OF THE TALENT PANELS, MORE SPECIFICALLY MAKE SURE THEY ARE SPAWNED IN THE CORRECT CORNER IN CASE OF MULTIPLAYER
    //A MAJOR PROBLEM WITH IT THO IS THAT IT DOESN'T SCALE WELL WITH DIFFERENT RESOLUTIONS BUT INSTEAD EXPECTS 1920x1080
    
    // Offset from the corners, adjust as needed
    private Vector2 offset = new Vector2(440,220); 

    void Start()
    {
        Transform playerTransform = transform.parent.parent;

        if (playerTransform != null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            Color color = Color.white; // Default color, change if necessary

            switch (playerTransform.name)
            {
                case "Player_1":
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0, 1);
                    rectTransform.anchoredPosition = new Vector2(offset.x, -offset.y);
                    color = new Color(0.7132076f, 0.3268512f, 0.3268512f, 0.6313726f); 
                    break;
                case "Player_2":
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.pivot = new Vector2(1, 1);
                    rectTransform.anchoredPosition = new Vector2(-offset.x, -offset.y);
                    color = new Color(0.3254902f, 0.351754f, 0.6117647f, 0.6313726f);
                    break;
                case "Player_3":
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    rectTransform.pivot = new Vector2(0, 0);
                    rectTransform.anchoredPosition = new Vector2(offset.x, offset.y);
                    color = new Color(0.5313726f, 0.8f, 0.3891499f, 0.5313726f);
                    break;
                case "Player_4":
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    rectTransform.pivot = new Vector2(1, 0);
                    rectTransform.anchoredPosition = new Vector2(-offset.x, offset.y);
                    color = new Color(0.5763184f, 0.3254902f, 0.6117647f, 0.6313726f); // Purple
                    break;
            }

            rectTransform.sizeDelta = new Vector2(100, 100);

            // Recursively find all "Background" GameObjects and change their colors
            ChangeBackgroundColor(transform, color);
        }
        else
        {
            Debug.LogError("Player grandparent not found.");
        }
    }

    void ChangeBackgroundColor(Transform parent, Color color)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "Background")
            {
                Image image = child.GetComponent<Image>();
                if (image != null)
                {
                    image.color = color;
                }
                else
                {
                    Debug.LogWarning("Image component not found on Background GameObject named: " + child.name);
                }
            }
            // Recursively call this method to check all descendants
            ChangeBackgroundColor(child, color);
            
            Destroy(this.GetComponent<TalentPanelPositioner>());
        }
    }
}
