using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingHack : MonoBehaviour, ITalentEffect
{
    private GameObject _meleeSlashGameObject;
    private MeleeSlash _meleeSlashComponent;

    public float healingHackRange;
    public void ApplyEffect(GameObject player)
    {
        _meleeSlashGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _meleeSlashComponent = _meleeSlashGameObject.GetComponent<MeleeSlash>();

        _meleeSlashComponent.healingHackEnabled = true;
        _meleeSlashComponent.healingHackRange = healingHackRange;
    }
}
