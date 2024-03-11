using System;
using System.Collections;
using UnityEngine;

public class BasicBulletController : MonoBehaviour
{
    //[SerializeField] private FloatVariable playerAttackSpeed;
    [SerializeField] private FloatVariable basicBulletSpeed;
    [SerializeField] private Vector3Variable cursorPosition;

    [SerializeField] private float damage = 10f;

    private float _bulletSpeed;
    private Vector3 _bulletDirection;
    private PlayerStatsController _playerStatsController;
    

    private void Start()
    {
        _playerStatsController = GameObject.FindWithTag("Player").GetComponent<PlayerStatsController>();
        
        transform.position = _playerStatsController.GetPlayerPosition();
        _bulletSpeed = basicBulletSpeed.value + _playerStatsController.GetAttackSpeed()/2; //TODO: Bullet speed should not be attackspeed
        _bulletDirection = (cursorPosition.value).normalized;

        StartCoroutine(SendBulletFlying());
        StartCoroutine(KillTimer());
    }

    private IEnumerator SendBulletFlying()
    {
        for (;;)
        {
            transform.Translate(_bulletDirection * (_bulletSpeed * Time.deltaTime));
            yield return null;
        }
    }

    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(damage);
                
                // Destroy the bullet
                Destroy(gameObject);
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}