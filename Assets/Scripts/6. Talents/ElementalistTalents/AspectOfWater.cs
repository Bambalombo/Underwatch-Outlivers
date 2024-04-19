using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectOfWater : MonoBehaviour, ITalentEffect
{
    private GameObject _elementalBoltGameObject;
    private ElementalBolts _elementalBoltComponent;
    public float healChance;
    
    public void ApplyEffect(GameObject player)
    {
        _elementalBoltGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _elementalBoltComponent = _elementalBoltGameObject.GetComponent<ElementalBolts>();

        _elementalBoltComponent.aspectOfWaterEnabled = true;
        _elementalBoltComponent.aspectOfWaterHealChance = healChance;
    }
}
