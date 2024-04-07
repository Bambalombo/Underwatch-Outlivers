using UnityEngine;

public class TalentPanelPositioner : MonoBehaviour
{
    //THIS SCRIPT IS PURELY MEANT TO HANDLE THE POSITIONS OF THE TALENT PANELS, MORE SPECIFICALLY MAKE SURE THEY ARE SPAWNED IN THE CORRECT CORNER IN CASE OF MULTIPLAYER
    //A MAJOR PROBLEM WITH IT THO IS THAT IT DOESN'T SCALE WELL WITH DIFFERENT RESOLUTIONS BUT INSTEAD EXPECTS 1920x1080
    
    // Offset from the corners, adjust as needed
    private Vector2 offset = new Vector2(500,250); 

    void Start()
    {
        Transform playerTransform = transform.parent.parent; // The grandparent of this GameObject

        if (playerTransform != null)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            switch (playerTransform.name)
            {
                case "Player_1": // Top Left
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);
                    rectTransform.pivot = new Vector2(0, 1);
                    rectTransform.anchoredPosition = new Vector2(offset.x, -offset.y);
                    break;
                case "Player_2": // Top Right
                    rectTransform.anchorMin = new Vector2(1, 1);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.pivot = new Vector2(1, 1);
                    rectTransform.anchoredPosition = new Vector2(-offset.x, -offset.y);
                    break;
                case "Player_3": // Bottom Left
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(0, 0);
                    rectTransform.pivot = new Vector2(0, 0);
                    rectTransform.anchoredPosition = new Vector2(offset.x, offset.y);
                    break;
                case "Player_4": // Bottom Right
                    rectTransform.anchorMin = new Vector2(1, 0);
                    rectTransform.anchorMax = new Vector2(1, 0);
                    rectTransform.pivot = new Vector2(1, 0);
                    rectTransform.anchoredPosition = new Vector2(-offset.x, offset.y);
                    break;
            }

            rectTransform.sizeDelta = new Vector2(100, 100); // Adjust the size according to your UI element's desired size

            // Iterate through children and set their position and size
            foreach (Transform child in transform)
            {
                RectTransform childRectTransform = child.GetComponent<RectTransform>();
                if (childRectTransform != null)
                {
                    childRectTransform.anchorMin = new Vector2(0, 0);
                    childRectTransform.anchorMax = new Vector2(1, 1);
                    childRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    childRectTransform.sizeDelta = Vector2.zero;
                    childRectTransform.anchoredPosition = Vector2.zero;
                }
            }
        }
        else
        {
            Debug.LogError("Player grandparent not found.");
        }
    }
}
