using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; // Bullet prefab to fire
    [SerializeField] private Transform rightFirePosition; // Right fire position
    [SerializeField] private Transform leftFirePosition; // Left fire position
    [SerializeField] private float bulletSpeed; // Speed of bullets
    [SerializeField] private float normalFireRate; // Bullets per second in normal state
    [SerializeField] private float normalAngleChange; // Angle change per bullet in normal state
    [SerializeField] private float rageFireRate; // Bullets per second in rage state
    [SerializeField] private float lastFireTime; // Time since the last fire
    [SerializeField] private float timeSinceLastBullet; // Time since the last bullet

    private Transform _currentFirePosition;
    
    private enum State { Normal, Rage }
    [SerializeField] private State currentState;
    [SerializeField] private GameObject[] players;

    private EnemyStatsController _enemyStatsController;
    private Transform _bulletParent;
    private SpriteRenderer _spriteRenderer;

    private float _currentAngle;
    private EnemySpawner _enemySpawner;

    private void Start()
    {
        _enemySpawner = GameManager.GetSpawnerEnemyControllerParent().GetComponent<EnemySpawner>();
        
        _enemyStatsController = GetComponent<EnemyStatsController>();
        players = GameManager.GetPlayerGameObjects();
        _bulletParent = GameManager.GetBulletParent().transform;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (_spriteRenderer.flipX)
        {
            _currentFirePosition = leftFirePosition;
        }
        else
        {
            _currentFirePosition = rightFirePosition;
        }
        
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
        
        
        if (_spriteRenderer.flipX && _currentFirePosition != leftFirePosition)
        {
            _currentFirePosition = leftFirePosition;
        }
        else if (!_spriteRenderer.flipX && _currentFirePosition != rightFirePosition)
        {
            _currentFirePosition = rightFirePosition;
        }
    }
    
    private void Shoot()
    {
        switch (currentState)
        {
            case State.Normal:
                if (Time.time >= lastFireTime + 1f / normalFireRate) // Use normalFireRate for Normal state
                {
                    NormalShootSpiral();
                    lastFireTime = Time.time;
                }
                break;
            case State.Rage:
                if (Time.time >= lastFireTime + 1f / rageFireRate) // Use rageFireRate for Rage state
                {
                    RageShootAtPlayers();
                    lastFireTime = Time.time;
                }
                break;
        }
        timeSinceLastBullet = 0f;
    }

    private void RageShootAtPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                Vector3 direction = (player.transform.position - _currentFirePosition.position).normalized;
                FireBullet(direction);
            }
        }
    }

    private void NormalShootSpiral()
    {
        _currentAngle += normalAngleChange;
        float angle = _currentAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        FireBullet(direction);
    }

    private void FireBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, _currentFirePosition.position, Quaternion.identity, _bulletParent);
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