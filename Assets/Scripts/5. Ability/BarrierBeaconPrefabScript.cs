using UnityEngine;

public class BarrierBeaconPrefabScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<WeaponStats>().SetDamage(other.GetComponentInChildren<WeaponStats>().GetDamage()*1.5f); //Idk hvor performance heavy det her er så ja
            other.GetComponentInChildren<AbilityStats>().SetDamage(other.GetComponentInChildren<AbilityStats>().GetDamage()*1.5f);
        }

        if (other.CompareTag("BossWeapon"))
        {
            Destroy(other.gameObject); //DESTROYS BOSS BULLETSSSSS :OOOOOOOOOOOOOOOOOOOOOOOOOO
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<WeaponStats>().SetDamage(other.GetComponentInChildren<WeaponStats>().GetDamage()/1.5f);
            other.GetComponentInChildren<AbilityStats>().SetDamage(other.GetComponentInChildren<AbilityStats>().GetDamage()/1.5f);
        }
    }
}