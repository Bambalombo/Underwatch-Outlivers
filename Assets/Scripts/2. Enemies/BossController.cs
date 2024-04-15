using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float normalStateDuration; // Duration of normal state in seconds
    [SerializeField] private float rageStateDuration; // Duration of rage state in seconds
    [SerializeField] private float bulletSpeed; // Speed of bullets
    [SerializeField] private float normalFireRate; // Bullets per second in normal state
    [SerializeField] private float rageFireRate; // Bullets per second in rage state
    [SerializeField] private float bulletDamage; // Damage of bullets
    [SerializeField] private float bulletLifetime; // Lifetime of bullets in seconds
    [SerializeField] private float lastFireTime = 0f; // Time since the last fire

    private enum State { Normal, Rage }
    [SerializeField] private State currentState;
    [SerializeField] private GameObject[] players;
    
    private EnemyStatsController enemyStatsController;
    private Transform _bulletParent;

    private void Start()
    {
        enemyStatsController = GetComponent<EnemyStatsController>();
        players = GameManager.GetPlayerGameObjects();
        StartCoroutine(StateMachine());
        
        bulletPrefab.GetComponent<BossBullet>().SetDamage(bulletDamage);
        _bulletParent = GameManager.GetBulletParent().transform;
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            if (enemyStatsController.GetCurrentHealth() <= enemyStatsController.GetMaxHealth() * 0.5f)
            {
                currentState = State.Rage;
                yield return new WaitForSeconds(rageStateDuration);
            }
            else
            {
                currentState = State.Normal;
                yield return new WaitForSeconds(normalStateDuration);
            }
        }
    }

    private void FixedUpdate()
    {
        if (enemyStatsController.GetIsFoundByPlayer())
        {
            if (Time.time >= lastFireTime + 1f / normalFireRate)
            {
                switch (currentState)
                {
                    case State.Normal:
                        NormalShootAtPlayers();
                        break;
                    case State.Rage:
                        RageShootSpiral();
                        break;
                }

                lastFireTime = Time.time;
            }
        }
    }

    private void NormalShootAtPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, _bulletParent);
                var bc = bullet.GetComponent<BulletController>();
                bc.Initialize(direction,bulletSpeed,enemyStatsController);
            }
        }
    }

    private void RageShootSpiral()
    {
        float angleStep = 360f / rageFireRate;
        for (int i = 0; i < rageFireRate; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, _bulletParent);
            StartCoroutine(MoveBullet(bullet, direction, bulletSpeed));
        }
    }

    private IEnumerator MoveBullet(GameObject bullet, Vector3 direction, float speed)
    {
        float elapsedTime = 0f;

        while (bullet != null && elapsedTime < bulletLifetime)
        {
            bullet.transform.position += direction * (speed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}