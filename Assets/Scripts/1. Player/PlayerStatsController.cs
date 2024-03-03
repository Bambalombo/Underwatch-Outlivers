using UnityEngine;
using System;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 10;
    [SerializeField] private int armor = 5;
    [SerializeField] private float speed = 5.0f;
    
    //***************** THIS SCRIPT IS NOT USED YET ******************//
    
    public int Health { get { return health; } set { health = value; } }
    public int AttackPower { get { return damage; } set { damage = value; } }
    public int Defense { get { return armor; } set { armor = value; } }
    public float Speed { get { return speed; } set { speed = value; } }

    public enum StatType
    {
        Health,
        Damage,
        Speed,
        Armor
    }
    

    public void LevelUpReward()
    {
        StatType[] stats = (StatType[])Enum.GetValues(typeof(StatType));
        StatType stat1 = stats[UnityEngine.Random.Range(0, stats.Length)];
        StatType stat2 = stats[UnityEngine.Random.Range(0, stats.Length)];
        StatType stat3 = stats[UnityEngine.Random.Range(0, stats.Length)];
        
    }

    public void UpgradeStat(StatType stat)
    {
        switch (stat)
        {
            case StatType.Health:
                Health += 10; 
                break;
            case StatType.Damage:
                AttackPower += 2; 
                break;
            case StatType.Speed:
                Speed += 0.5f; 
                break;
            case StatType.Armor:
                Defense += 1; 
                break;
        }
    }
}