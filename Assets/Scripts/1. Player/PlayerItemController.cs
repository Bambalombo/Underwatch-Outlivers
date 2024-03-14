using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField] private GameObject weaponParent; // The parent of the weapons
    [SerializeField] private List<GameObject> weapons; // List of weapons the player has

    private void Awake()
    {
        weapons = new List<GameObject>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("WeaponPickup")) return;

        var weaponPickup = collision.gameObject.GetComponent<WeaponPickup>();
        var weaponPrefab = weaponPickup.GetWeaponPrefab();

        var weapon = Instantiate(weaponPrefab, weaponParent.transform.position, Quaternion.identity, weaponParent.transform);
        weapons.Add(weapon);

        // Destroy the weapon pickup
        Destroy(collision.gameObject);
    }
}