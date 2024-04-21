using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalSpread : MonoBehaviour, ITalentEffect
{
    private GameObject _elementalBoltGameObject;
    private ElementalBolts _elementalBoltComponent;
    
    public void ApplyEffect(GameObject player)
    {
        _elementalBoltGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _elementalBoltComponent = _elementalBoltGameObject.GetComponent<ElementalBolts>();

        _elementalBoltComponent.elementalSpreadEnabled = true;
    }
}

