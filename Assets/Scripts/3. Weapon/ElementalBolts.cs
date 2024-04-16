using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ElementalBolts : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private FloatVariable basicBulletSpeed;
    private NearestEnemyFinder _nearestEnemyFinder;
    private WeaponStats _weaponStats;

    public AoeDamagePool AoeDamagePool;
    
    //Talent bools
    public bool aspectOfEarthEnabled;

    private void Awake()
    {
        _weaponStats = GetComponent<WeaponStats>();
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
    }

    private void OnEnable()
    {
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
            
            if (aspectOfEarthEnabled)
            {
                bulletController.OnBulletHitEnemy += SpawnAspectOfEarthAoePool;
            }
        
            // Pass _weaponStats to the Initialize method
            bulletController.Initialize(direction, basicBulletSpeed.value, _weaponStats);
        }
    }

    private void SpawnAspectOfEarthAoePool(GameObject enemy)
    {
        AoeDamagePool.AttemptInitialize(_weaponStats.GetDamage()/5, enemy.transform.position);
    }
}
