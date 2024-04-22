using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSphereTalent : MonoBehaviour, ITalentEffect
{
    private GameObject _droneGameObject;
    private WeaponDrone _droneComponent;
    public void ApplyEffect(GameObject player)
    {
        _droneGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _droneComponent = _droneGameObject.GetComponent<WeaponDrone>();

        _droneComponent.shockSphereEnabled = true;
    }
}
