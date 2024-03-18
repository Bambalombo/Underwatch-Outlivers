using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponPickup : MonoBehaviour
{

    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("WeaponPickup")) return;

        var weaponPickup = collision.gameObject.GetComponent<WeaponPickup>();
        var weaponPrefab = weaponPickup.GetWeaponPrefab();

        var weapon = Instantiate(weaponPrefab, weaponParent.transform.position, Quaternion.identity, weaponParent.transform);
        weapons.Add(weapon);

        // Destroy the weapon pickup
        Destroy(collision.gameObject);
    }*/
}