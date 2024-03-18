using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Assignments")]
    [SerializeField] private GameObject weaponParent; // The parent of the weapons
    [SerializeField] private PlayerStatsController playerStatsController;
    
    [Header("Default Class Weapons")]
    [SerializeField] private GameObject elementalistWeapon;
    [SerializeField] private GameObject voidwalkerWeapon;
    [SerializeField] private GameObject mutantBerserkerWeapon;
    [SerializeField] private GameObject xenobiologistWeapon;
    [SerializeField] private GameObject xi017Weapon;
    
    [Header("Currently Active Weapon")]
    [SerializeField] private List<GameObject> activeWeapons; // List of weapons the player has

    private void Awake()
    {
        SetPlayerWeapon();
    }

    private void SetPlayerWeapon()
    {
        activeWeapons = new List<GameObject>();

        GameObject weapon = null;
        switch (playerStatsController.playerClass)
        {
            case PlayerStatsController.PlayerClass.Elementalist:
                weapon = Instantiate(elementalistWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Voidwalker:
                weapon = Instantiate(voidwalkerWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.MutantBerserker:
                weapon = Instantiate(mutantBerserkerWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.Xenobiologist:
                weapon = Instantiate(xenobiologistWeapon, parent: weaponParent.transform);
                break;
            case PlayerStatsController.PlayerClass.XI_017:
                weapon = Instantiate(xi017Weapon, parent: weaponParent.transform);
                break;
            default:
                Debug.Log("Class not detected. No weapon was assigned.");
                break;
        }
        if (weapon)
            activeWeapons.Add(weapon);
    }
    
    
    

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