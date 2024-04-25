using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierBeaconPrefabScript : MonoBehaviour
{
    private Dictionary<Collider2D, Color> originalColors = new Dictionary<Collider2D, Color>();
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

            SpriteRenderer playerSprite = other.GetComponent<SpriteRenderer>();
            if (playerSprite != null)
            {
                // Store the original color in the dictionary
                originalColors[other] = playerSprite.color;
                // Set the sprite color to blue while preserving the alpha
                playerSprite.color = new Color(0, 0.6f, 1, playerSprite.color.a);
            }
            
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

            SpriteRenderer playerSprite = other.GetComponent<SpriteRenderer>();
            if (playerSprite != null && originalColors.ContainsKey(other))
            {
                // Revert to the original color stored when entering
                playerSprite.color = originalColors[other];
                originalColors.Remove(other); // Remove the entry from the dictionary
            }

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
            player.GetComponentInChildren<PlayerHealthController>().PlayerHeal(casterPlayerStats.GetMaxHealth()*0.05f); //Heals by 1% of casting players max health
            yield return new WaitForSeconds(1f); // Heal every second
        }
    }
}
