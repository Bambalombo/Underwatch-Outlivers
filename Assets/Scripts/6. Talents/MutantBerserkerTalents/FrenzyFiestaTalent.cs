using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrenzyFiestaTalent : MonoBehaviour, ITalentEffect
{
    private GameObject _frenziedMutationGameObject;
    private FrenziedMutation _frenziedMutationComponent;
    public void ApplyEffect(GameObject player)
    {
        _frenziedMutationGameObject = player.GetComponent<ClassAssets>().GetActiveAbilities();
        _frenziedMutationComponent = _frenziedMutationGameObject.GetComponent<FrenziedMutation>();

        _frenziedMutationComponent.frenzyFiestaEnabled = true;
    }
}
