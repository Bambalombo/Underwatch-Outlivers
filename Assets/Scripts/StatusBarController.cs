using UnityEngine;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour
{
    [SerializeField] private Slider statusBar;
    //[SerializeField] private Transform parentObject;
    //[SerializeField] private Vector3 positionOffset;
    

    public void UpdateStatusBar(float currentValue, float maxValue)
    {
        statusBar.value = currentValue / maxValue;
    }
}
