using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryKitten : MonoBehaviour, ITalentEffect
{
    private GameObject _voidBlastGameobject;

    public void ApplyEffect(GameObject player)
    {
        _voidBlastGameobject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        //_voidBlastGameobject.GetComponent<VoidBlast>().travelDistance = 0.1f;
        _voidBlastGameobject.GetComponent<WeaponStats>().SetAttackRange(_voidBlastGameobject.GetComponent<WeaponStats>().GetAttackRange()*2f);
        _voidBlastGameobject.GetComponent<VoidBlast>().angryKittenActive = true;

    }
}
