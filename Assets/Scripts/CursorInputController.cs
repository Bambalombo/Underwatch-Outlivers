using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CursorInputController : MonoBehaviour
{
    [SerializeField] private Vector3Variable cursorPosition;
    private Vector3 _screenCenter;

    public GameObject textObeject;
    private TextMeshProUGUI tmp;

    private void Awake()
    {
        _screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
        tmp = textObeject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (cursorPosition.value != Input.mousePosition)
            UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        cursorPosition.value = Input.mousePosition - _screenCenter;
        tmp.text = $"{cursorPosition.value}\n{Mathf.Atan2(cursorPosition.value.normalized.y, cursorPosition.value.normalized.x) * Mathf.Rad2Deg}";
    }
}
