using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI  timeText;
    [SerializeField] private FloatVariable gameTime;

    void Start()
    {
        gameTime.value = 0;
        
        timeText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        gameTime.value += Time.deltaTime;
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime.value / 60);
        int seconds = Mathf.FloorToInt(gameTime.value % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}