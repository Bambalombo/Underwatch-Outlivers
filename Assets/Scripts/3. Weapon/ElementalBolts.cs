using System.Collections;
using UnityEngine;

public class ElementalBolts : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private FloatVariable basicBulletSpeed;
    private NearestEnemyFinder _nearestEnemyFinder;
    private WeaponStats _weaponStats;

    private void Start()
    {
        _weaponStats = GetComponent<WeaponStats>();
        
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while (true)
        {
            SpawnAndInitializeBullet();
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
        }
    }

    private void SpawnAndInitializeBullet()
    {
        // Find the nearest enemy from the player's position
        GameObject nearestEnemy = _nearestEnemyFinder.GetNearestEnemy(transform.position);
    
        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
        
            // Pass _weaponStats to the Initialize method
            bulletController.Initialize(direction, basicBulletSpeed.value, _weaponStats);
        }
    }
}
