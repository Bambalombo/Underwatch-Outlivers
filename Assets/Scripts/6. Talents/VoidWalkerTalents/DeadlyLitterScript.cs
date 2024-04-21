using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyLitterScript : MonoBehaviour, ITalentEffect
{

    private GameObject _abyssalRiftGameObject;
    public void ApplyEffect(GameObject player)
    {
        _abyssalRiftGameObject = player.GetComponent<ClassAssets>().GetActiveAbilities();
        _abyssalRiftGameObject.GetComponent<AbyssalRift>().deadlyLitterActivated = enabled;

    }
}
