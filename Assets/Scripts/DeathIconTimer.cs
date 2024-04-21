using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DeathIconTimer : MonoBehaviour
{
    [SerializeField] private GameObject deathIconPrefab;
    [SerializeField] private int textIndex = 5;
    [SerializeField] private int iconDistance = 100;
    private GameObject[] _deathIcons;
    private TextMeshProUGUI[] _deathTimerTexts;
    private PlayerStatsController[] _playerStatsControllers;
    private TextMeshProUGUI _deathText;
    private Coroutine _deathTextCoroutine;
    
    
    
    private void Start()
    {
        _deathText = GameObject.FindWithTag("DeathTextTag").GetComponent<TextMeshProUGUI>();
        _deathText.enabled = false;
    }
    
    private void OnEnable()
    {
        GameManager.OnPlayerDeath += ShowDeathIcon;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerDeath -= ShowDeathIcon;
    }
    
    public void CreateDeathIcons(int numberOfPlayers, List<int> classSelections)
    {
        var icons = new GameObject[numberOfPlayers];
        _deathTimerTexts = new TextMeshProUGUI[numberOfPlayers];
        _playerStatsControllers = new PlayerStatsController[numberOfPlayers];
        
        float totalWidth = (numberOfPlayers - 1) * iconDistance; // 200 is the distance between the icon centers
        float startPosition = -totalWidth / 2 + (Screen.width / 2) - iconDistance/2; // 100 is the offset from the center of the screen
        
        for (int i = 0; i < numberOfPlayers; i++)
        {
            var icon = Instantiate(deathIconPrefab, transform.position, Quaternion.identity);
            icon.transform.SetParent(transform);
            
            // We activate the image corresponding to the selected class index
            icon.transform.GetChild(classSelections[i]).gameObject.SetActive(true);
            icon.name = "Player_" + (i + 1) + "Icon";

            _deathTimerTexts[i] = icon.transform.GetChild(textIndex).GetComponent<TextMeshProUGUI>();
            _playerStatsControllers[i] = GameManager.GetPlayerGameObjects()[i].GetComponent<PlayerStatsController>();
            icons[i] = icon;
            
            Vector3 iconPosition = icon.transform.position;
            iconPosition.x = startPosition + i * iconDistance;
            icon.transform.position = iconPosition;
            
            icon.SetActive(false);
        }

        _deathIcons = icons;
    }

    
    private void ShowDeathIcon(int playerIndex, float respawnTime)
    {
        if (playerIndex >= 0 && playerIndex < _deathIcons.Length)
        {
            if (_deathTextCoroutine != null)
                StopCoroutine(_deathTextCoroutine);
            
            _deathTextCoroutine =StartCoroutine(ShowDeathText(playerIndex));
            StartCoroutine(ShowDeathIconWhileDead(playerIndex, respawnTime));
        }
    }

    private IEnumerator ShowDeathText(int playerIndex)
    {
        _deathText.enabled = true;
        _deathText.text = "Player " + (playerIndex + 1) + " Died!";
        _deathText.CrossFadeAlpha(255f, 0.1f, false);
        
        yield return new WaitForSeconds(2f);
        
        _deathText.CrossFadeAlpha(0, 1f, false);
        _deathText.enabled = false;
    }

    private IEnumerator ShowDeathIconWhileDead(int playerIndex, float respawnTime)
    {
        _deathIcons[playerIndex].SetActive(true);
        var text = _deathTimerTexts[playerIndex];
        var deathDuration = respawnTime;
        
        while (deathDuration > 0)
        {
            text.text = deathDuration.ToString(CultureInfo.InvariantCulture);
            yield return new WaitForSeconds(1f);
            deathDuration--;
        }
        
        _deathIcons[playerIndex].SetActive(false);
    }
}
