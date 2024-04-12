using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryKitten : MonoBehaviour, ITalentEffect
{
    private GameObject _voidBlastGameobject;

    public void ApplyEffect(GameObject player)
    {
        _voidBlastGameobject = player.GetComponent<ClassAssets>().GetActiveWeapons();
        _voidBlastGameobject.GetComponent<WeaponStats>().SetAttackRange(0);
        Vector2 currentScale = _voidBlastGameobject.GetComponent<VoidBlast>().projectileScale;
        Vector2 increasedScale = currentScale * 1.5f; // Multiply each component by 1.5
        _voidBlastGameobject.GetComponent<VoidBlast>().projectileScale = increasedScale;

    }
}
