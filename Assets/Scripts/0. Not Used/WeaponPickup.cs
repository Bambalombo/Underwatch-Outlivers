using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    
    public GameObject GetWeaponPrefab() { return weaponPrefab; }
}
