using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodtype00 : MonoBehaviour, ITalentEffect
{
    private GameObject _meleeSlashGameObject;
    private MeleeSlash _meleeSlashComponent;

    public void ApplyEffect(GameObject player)
    {
        _meleeSlashGameObject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _meleeSlashGameObject.GetComponent<AoeDamagePool>().enabled = true;
        _meleeSlashComponent = _meleeSlashGameObject.GetComponent<MeleeSlash>();

        _meleeSlashComponent.bloodType00Enabled = true;
    }
}
