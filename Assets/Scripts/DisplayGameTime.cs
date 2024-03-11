using TMPro;
using UnityEngine;

public class GameTimeDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI  timeText;
    [SerializeField] private FloatVariable gameTime;

    private void Start()
    {
        gameTime.value = 0;
        
        timeText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        gameTime.value += Time.deltaTime;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(gameTime.value / 60);
        int seconds = Mathf.FloorToInt(gameTime.value % 60);
        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}