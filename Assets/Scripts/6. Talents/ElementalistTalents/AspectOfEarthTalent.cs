using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectOfEarthTalent : MonoBehaviour, ITalentEffect
{
    private GameObject _elementalBoltsGameObject;
    private ElementalBolts _elementalBoltsComponent;
    public void ApplyEffect(GameObject player)
    {
        _elementalBoltsGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _elementalBoltsComponent = _elementalBoltsGameObject.GetComponent<ElementalBolts>();

        _elementalBoltsComponent.aspectOfEarthEnabled = true;
    }
}
