using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private RawImage redTint;
    [SerializeField] private CanvasGroup normalUI;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverUI;
    [FormerlySerializedAs("enterHighscore")] [SerializeField] private GameObject saveHighscoreUI;
    [FormerlySerializedAs("viewHighscore")] [SerializeField] private GameObject viewHighscoreUI;
    
    [Header("Fade Values")]
    [SerializeField] private float targetTimeScale = 0.05f;
    [SerializeField] private float timeSlowDuration = 0.5f;
    [SerializeField] private float uiFadeDuration = 2f; 
    
    [Header("Highscore References")]
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI currentHighscoreText;
    [SerializeField] private TextMeshProUGUI allHighscoresText;
    [SerializeField] private Button saveHighscoreButton;
    [SerializeField] private Button viewHighscoreButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private FloatVariable gameTime;
    [SerializeField] private List<Highscore> highscores = new List<Highscore>();

    // private FileStream stream;
    
    private void Start()
    {
        SetupReferences();
        SetupListeners();
        // SetupDefaultVisibilities();
        
        Debug.Log($"highscores.dat will save at '{Application.persistentDataPath}''");
    }
    
    private void OnEnable()
    {
        GameManager.OnGameOver += StartGameOverSequence;
    }
    
    private void OnDisable()
    {
        GameManager.OnGameOver -= StartGameOverSequence;
    }

    void SetupReferences()
    {
        normalUI = transform.GetChild(0).gameObject.GetComponent<CanvasGroup>();
        gameOverCanvas = transform.GetChild(1).gameObject;
        redTint = gameOverCanvas.transform.GetChild(0).GetComponent<RawImage>();
        gameOverUI = gameOverCanvas.transform.GetChild(1).gameObject;
        saveHighscoreUI = gameOverUI.transform.GetChild(1).gameObject;
        viewHighscoreUI = gameOverUI.transform.GetChild(2).gameObject;
    }

    void SetupListeners()
    {
        saveHighscoreButton.onClick.AddListener(SaveHighscore);
        viewHighscoreButton.onClick.AddListener(SkipSaveHighscore);
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    void SetupDefaultVisibilities()
    {
        viewHighscoreUI.SetActive(false);
        saveHighscoreUI.SetActive(false);
        gameOverCanvas.SetActive(false);
    }

    void StartGameOverSequence()
    {
        StartCoroutine(GameOverSequence());
    }
    
    private IEnumerator GameOverSequence()
    {
        gameOverCanvas.SetActive(true);

        StartCoroutine(SlowDownTime(timeSlowDuration));
        yield return StartCoroutine(FadeOutGameUI(uiFadeDuration));

        currentHighscoreText.text = "You survived for\n" + GameTimeAsText(gameTime.value);
        
        gameOverUI.SetActive(true);
    }
    
    IEnumerator SlowDownTime(float duration)
    {
        float start = Time.realtimeSinceStartup;
        float startScale = Time.timeScale;
        
        while (Time.realtimeSinceStartup < start + duration)
        {
            var scale = (Time.realtimeSinceStartup - start) / duration;
            
            Time.timeScale = Mathf.Lerp(startScale, targetTimeScale, scale);
            normalUI.alpha = 1 - scale;
            
            yield return null;
        }

        Time.timeScale = targetTimeScale;
        normalUI.alpha = 0;
    }
    
    IEnumerator FadeOutGameUI(float duration)
    {
        float start = Time.realtimeSinceStartup;
        Color color = redTint.color;
        
        while (Time.realtimeSinceStartup < start + duration)
        {
            var scale = (Time.realtimeSinceStartup - start) / duration;
            
            color.a = scale / 3;
            redTint.color = color;
            
            yield return null;
        }
    }
    
    private string GameTimeAsText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    
    private string GameTimeAsText(string time)
    {
        int minutes = Mathf.FloorToInt(float.Parse(time) / 60);
        int seconds = Mathf.FloorToInt(float.Parse(time) % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    
    private void SaveHighscore()
    {
        string playerName = playerNameInputField.text;
        string score = gameTime.value.ToString();
        string realWorldDateTime = DateTime.Now.ToString("dd.MM.yyyy, HH:mm");

        Highscore newHighscore = new Highscore(playerName, score, realWorldDateTime);

        string path = Application.persistentDataPath + "/highscores.dat";
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream;

        if (File.Exists(path))
        {
            stream = new FileStream(path, FileMode.Open);
            highscores = formatter.Deserialize(stream) as List<Highscore>;
            stream.Close();
        }

        highscores.Add(newHighscore);

        stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, highscores);
        stream.Close();

        saveHighscoreUI.SetActive(false);
        viewHighscoreUI.SetActive(true);
        gameOverUI.transform.GetChild(0).gameObject.SetActive(false);
        ViewHighscores();
    }
    
    void SkipSaveHighscore()
    {
        saveHighscoreUI.SetActive(false);
        viewHighscoreUI.SetActive(true);
        gameOverUI.transform.GetChild(0).gameObject.SetActive(false);
        ViewHighscores();
    }
    
    private void ViewHighscores()
    {
        string path = Application.persistentDataPath + "/highscores.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            highscores = formatter.Deserialize(stream) as List<Highscore>;
            stream.Close();
            
            highscores?.Sort((x, y) =>
                TimeSpan.Parse(GameTimeAsText(y.gameTime)).CompareTo(TimeSpan.Parse(GameTimeAsText(x.gameTime)))
            );
            
            allHighscoresText.text = "";
            foreach (Highscore highscore in highscores)
            {
                allHighscoresText.text += $"{highscore.realWorldDateTime} - {highscore.playerName} - {GameTimeAsText(float.Parse(highscore.gameTime))}\n";
            }
        }
        else
        {
            Debug.LogError("Highscore file not found");
        }
    }

    void ReturnToMainMenu()
    {
        // Reset the game
        GameManager.DestroyInstance(); // Destroy the GameManager instance
        
        Time.timeScale = 1; // Reset the time scale
        
        SceneManager.LoadScene("Menu");
    }
}

[System.Serializable]
public class Highscore
{
    public string playerName;
    public string gameTime;
    public string realWorldDateTime;

    public Highscore(string playerName, string gameTime, string realWorldDateTime)
    {
        this.playerName = playerName;
        this.gameTime = gameTime;
        this.realWorldDateTime = realWorldDateTime;
    }
}