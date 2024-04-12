using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulHarvestTalent : MonoBehaviour, ITalentEffect
{
    //This talent adds a chance for players inside Voidwalkers abyssal rift ability to heal the player for a small amount
    public float HealAmount;
    public float HealChance;

    private GameObject _abyssalRiftGameObject;
    public void ApplyEffect(GameObject player)
    {
        _abyssalRiftGameObject = player.GetComponent<ClassAssets>().GetActiveAbilities();
        _abyssalRiftGameObject.GetComponent<AbyssalRift>().soulHarvestTalentActivated = true;
        _abyssalRiftGameObject.GetComponent<AbyssalRift>().soulHarvestHealAmount = HealAmount;
        _abyssalRiftGameObject.GetComponent<AbyssalRift>().soulHarvestHealChance = HealChance;

    }
}
