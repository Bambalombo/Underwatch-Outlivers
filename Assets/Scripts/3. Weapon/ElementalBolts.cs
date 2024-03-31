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
            BulletController bulletController = bullet.AddComponent<BulletController>();
        
            // Pass _weaponStats to the Initialize method
            bulletController.Initialize(direction, basicBulletSpeed.value, _weaponStats);
        }
    }

    public class BulletController : MonoBehaviour
    {   
        private Vector3 _direction;
        private WeaponStats _weaponStats; // Store weapon stats

        // Modified Initialize method to accept WeaponStats
        public void Initialize(Vector3 direction, float speed, WeaponStats weaponStats)
        {
            _direction = direction;
            _weaponStats = weaponStats; // Store the passed weapon stats
            StartCoroutine(SendBulletFlying());
        }

        private IEnumerator SendBulletFlying()
        {
            while (true)
            {
                transform.Translate(_direction * (_weaponStats.GetProjectileSpeed() * Time.deltaTime), Space.World);
                yield return null;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                var enemy = collision.gameObject.GetComponent<EnemyCombatController>();
                if (enemy != null)
                {
                    enemy.EnemyTakeDamage(_weaponStats.GetDamage()); // Adjust '10' if needed
                    Destroy(gameObject);
                }
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
