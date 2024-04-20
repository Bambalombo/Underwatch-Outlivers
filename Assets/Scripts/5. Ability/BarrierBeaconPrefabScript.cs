using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBeaconPrefabScript : MonoBehaviour
{
    public bool HealingHaven;
    public bool MobileMantle;
    public Transform casterTransform;
    public PlayerStatsController casterPlayerStats; //The stats of the player who cast the ability

    private Dictionary<Collider2D, Coroutine> activeHealings = new Dictionary<Collider2D, Coroutine>();

    private void Update()
    {
        if (MobileMantle && casterTransform != null)
        {
            transform.position = casterTransform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyBuff(other);

            if (HealingHaven)
            {
                // Start healing if not already healing
                if (!activeHealings.ContainsKey(other))
                {
                    Coroutine healing = StartCoroutine(HealPlayerPeriodically(other));
                    activeHealings.Add(other, healing);
                }
            }
        }

        if (other.CompareTag("BossWeapon"))
        {
            Destroy(other.gameObject); // Destroys boss weapon projectiles
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            RemoveBuff(other);

            if (HealingHaven && activeHealings.ContainsKey(other))
            {
                StopCoroutine(activeHealings[other]);
                activeHealings.Remove(other);
            }
        }
    }

    private void ApplyBuff(Collider2D player)
    {
        var weaponStats = player.GetComponentInChildren<WeaponStats>();
        var abilityStats = player.GetComponentInChildren<AbilityStats>();
        if (weaponStats != null && abilityStats != null)
        {
            weaponStats.SetDamage(weaponStats.GetDamage() * 1.5f);
            abilityStats.SetDamage(abilityStats.GetDamage() * 1.5f);
        }
    }

    private void RemoveBuff(Collider2D player)
    {
        var weaponStats = player.GetComponentInChildren<WeaponStats>();
        var abilityStats = player.GetComponentInChildren<AbilityStats>();
        if (weaponStats != null && abilityStats != null)
        {
            weaponStats.SetDamage(weaponStats.GetDamage() / 1.5f);
            abilityStats.SetDamage(abilityStats.GetDamage() / 1.5f);
        }
    }

    private IEnumerator HealPlayerPeriodically(Collider2D player)
    {
        while (true)
        {
            player.GetComponentInChildren<PlayerHealthController>().PlayerHeal(casterPlayerStats.GetMaxHealth()*0.01f); //Heals by 1% of casting players max health
            yield return new WaitForSeconds(1f); // Heal every second
        }
    }
}
