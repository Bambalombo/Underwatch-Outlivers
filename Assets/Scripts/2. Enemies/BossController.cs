using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BossController : MonoBehaviour
{
    // SerializeField attributes allow these variables to be adjusted in the Unity Inspector
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed; // Speed of bullets
    [SerializeField] private float normalFireRate; // Bullets per second in normal state
    [SerializeField] private float rageFireRate; // Bullets per second in rage state
    [SerializeField] private float lastFireTime; // Time since the last fire
    [SerializeField] private float timeSinceLastBullet; // Time since the last bullet

    private enum State { Normal, Rage }
    [SerializeField] private State currentState;
    [SerializeField] private GameObject[] players;

    private EnemyStatsController _enemyStatsController;
    private Transform _bulletParent;

    private float _currentAngle;
    private EnemySpawner _enemySpawner;

    private void Start()
    {
        _enemySpawner = GameManager.GetSpawnerEnemyControllerParent().GetComponent<EnemySpawner>();
        
        _enemyStatsController = GetComponent<EnemyStatsController>();
        players = GameManager.GetPlayerGameObjects();
        _bulletParent = GameManager.GetBulletParent().transform;
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            float currentHealth = _enemyStatsController.GetCurrentHealth();
            float maxHealth = _enemyStatsController.GetMaxHealth();

            if (currentHealth <= maxHealth * 0.5f)
            {
                currentState = State.Rage;
            }
            else
            {
                currentState = State.Normal;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void FixedUpdate()
    {
        if (!_enemyStatsController.GetIsFoundByPlayer()) return;
        
        timeSinceLastBullet += Time.fixedDeltaTime;
        if (Time.time >= lastFireTime + 1f / normalFireRate)
        {
            Shoot();
            lastFireTime = Time.time;
        }
    }
    
    private void Shoot()
    {
        switch (currentState)
        {
            case State.Normal:
                ShootSpiral();
                break;
            case State.Rage:
                ShootAtPlayers();
                break;
        }
        timeSinceLastBullet = 0f;
    }

    private void ShootAtPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                FireBullet(direction);
            }
        }
    }

    private void ShootSpiral()
    {
        float angleStep = 360f / rageFireRate;
        _currentAngle += angleStep;
        float angle = _currentAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        FireBullet(direction);
    }

    private void FireBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, _bulletParent);
        var bc = bullet.GetComponent<BulletController>();
        bc.Initialize(direction, bulletSpeed, _enemyStatsController);

        // Calculate the angle of the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        angle += 135;

        // Set the rotation of the bullet to match the angle
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }


    private void OnDestroy()
    {
        _enemySpawner.RemoveEnemyFromList(gameObject);
    }
}