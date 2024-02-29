using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    public TextMeshProUGUI  timeText;
    private float gameTime;

    void Start()
    {
        gameTime = 0;
        
        timeText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        gameTime += Time.deltaTime;
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}