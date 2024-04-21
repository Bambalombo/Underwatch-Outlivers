using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ElementalBolts : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    [SerializeField] private FloatVariable basicBulletSpeed;
    private NearestEnemyFinder _nearestEnemyFinder;
    private WeaponStats _weaponStats;
    private GameObject _playerGameObject;

    public AoeDamagePool AoeDamagePool;
    
    public float enemyDetectionRange = 25f;
    
    //Talent variables
    public bool aspectOfEarthEnabled;
    public bool aspectOfWaterEnabled;
    [FormerlySerializedAs("healingBulletPrefab")] [SerializeField] private GameObject aspectOfWaterBullet; // Prefab for the healing bullet
    public float aspectOfWaterHealChance;
    public bool elementalSpreadEnabled;
    
    [SerializeField]private AudioSource audioSource;

    private void Awake()
    {
        _playerGameObject = transform.parent.parent.gameObject;
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
        int numberOfTargets = elementalSpreadEnabled ? 3 : 1;  // Number of bullets/enemies to target
        List<GameObject> closestEnemies = _nearestEnemyFinder.GetClosestEnemies(transform.position, numberOfTargets, maxRange:enemyDetectionRange);
        int enemiesCount = closestEnemies.Count;
    
        if (enemiesCount == 0)
        {
            return; // Exit the method if no enemies are found
        }
    
        for (int i = 0; i < numberOfTargets; i++) 
        {
            GameObject target = i < enemiesCount ? closestEnemies[i] : closestEnemies[0];  // Use the first closest if not enough enemies found
    
            if (target != null)  
            {
                Vector3 direction = (target.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                audioSource.Play();
                BulletController bulletController = bullet.GetComponent<BulletController>();
    
                if (aspectOfEarthEnabled)
                {
                    bulletController.OnBulletHitEnemy += SpawnAspectOfEarthAoePool;
                }
    
                if (aspectOfWaterEnabled)
                {
                    bulletController.OnBulletHitEnemy += SpawnAspectOfWaterHealingStream;
                }
    
                bulletController.Initialize(direction, basicBulletSpeed.value, _weaponStats);
            }
        }
    }





    private void SpawnAspectOfEarthAoePool(GameObject enemy)
    {
        AoeDamagePool.AttemptInitialize(_weaponStats.GetDamage() / 5, enemy.transform.position);
    }

    private void SpawnAspectOfWaterHealingStream(GameObject enemy)
    {
        if (Random.value < aspectOfWaterHealChance) // 50% chance
        {
            GameObject healingBullet = Instantiate(aspectOfWaterBullet, enemy.transform.position, Quaternion.identity);
            HealingBulletController healingController = healingBullet.GetComponent<HealingBulletController>();

            // Pass the transform of the player who is firing the bullet
            healingController.Initialize(_playerGameObject.transform, 20f, _weaponStats);
        }
    }

}
