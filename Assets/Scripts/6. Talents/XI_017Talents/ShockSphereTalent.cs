using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSphereTalent : MonoBehaviour, ITalentEffect
{
    private GameObject _droneGameObject;
    private FrenziedMutation _droneComponent;
    public void ApplyEffect(GameObject player)
    {
        _droneGameObject = player.GetComponent<ClassAssets>().GetActiveAbilities();
        _droneComponent = _droneGameObject.GetComponent<FrenziedMutation>();

        _droneComponent.frenzyFiestaEnabled = true;
    }
}
