using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemType pickupType = ItemType.None;
    [SerializeField] private float value = 10; // Set the amount of health/experience/gold etc. to give

    public enum ItemType
    {
        None,
        Health,
        Experience
    }
    
    public bool isBeingPickedUp { get; set; }
    private GameObject _playerTarget;
    private Coroutine _checkIfTargetIsActiveCoroutine;

    public float GetValue()
    {
        return value;
    }
    
    public ItemType GetItemType()
    {
        return pickupType;
    }
    
    public void CheckIfBeingPickedUp(GameObject target)
    {
        _playerTarget = target;
       
        if (!isBeingPickedUp && _checkIfTargetIsActiveCoroutine == null)
        {
            isBeingPickedUp = true;
            _checkIfTargetIsActiveCoroutine = StartCoroutine(CheckIfTargetIsActive());
        }
    }

    public IEnumerator CheckIfTargetIsActive()
    {
        while (_playerTarget.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        isBeingPickedUp = false;
        _checkIfTargetIsActiveCoroutine = null;
    }
}
