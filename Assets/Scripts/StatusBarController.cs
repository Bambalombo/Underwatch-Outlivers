using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    [SerializeField] private Slider statusBar;
    [SerializeField] private Transform parentObject;
    [SerializeField] private Vector3 positionOffset;

    private void Start()
    {
        transform.position = parentObject.position + positionOffset;
    }

    public void UpdateStatusBar(float currentValue, float maxValue)
    {
        statusBar.value = currentValue / currentValue;
    }
}
