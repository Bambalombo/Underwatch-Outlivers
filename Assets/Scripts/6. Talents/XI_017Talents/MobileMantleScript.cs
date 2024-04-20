using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileMantleScript : MonoBehaviour, ITalentEffect
{
    private GameObject _barrierBeaconGameObject;
    private BarrierBeacon _barrierBeaconComponent;
    public void ApplyEffect(GameObject player)
    {
        _barrierBeaconGameObject = player.GetComponent<ClassAssets>().GetActiveAbilities();
        _barrierBeaconComponent = _barrierBeaconGameObject.GetComponent<BarrierBeacon>();

        _barrierBeaconComponent.mobileMantleActive = true;
    }
}

