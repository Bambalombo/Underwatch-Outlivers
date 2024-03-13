using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPlayersMenuBehavior : MonoBehaviour
{   
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button increaseButton;

    [SerializeField] private TMPro.TextMeshProUGUI numberOfPlayersTextAmount;
    
    void Start()
    {
        numberOfPlayersTextAmount.text = GameManager.GetNumberOfPlayers().ToString();
        decreaseButton.onClick.AddListener(() =>
        {
            GameManager.SetNumberOfPlayers(GameManager.GetNumberOfPlayers() - 1);
            numberOfPlayersTextAmount.text = GameManager.GetNumberOfPlayers().ToString();
        });
        increaseButton.onClick.AddListener(() =>
        {
            GameManager.SetNumberOfPlayers(GameManager.GetNumberOfPlayers() + 1);
            numberOfPlayersTextAmount.text = GameManager.GetNumberOfPlayers().ToString();
        });
    }


}
