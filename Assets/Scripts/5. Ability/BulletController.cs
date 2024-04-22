using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float destroyTime = 10f;
    private float impactDestroyTime;

    private Vector3 _direction;
    private float _speed;
    private float _damage;

    // Define a delegate and an event for bullet collision
    public delegate void BulletHitHandler(GameObject enemy);
    public event BulletHitHandler OnBulletHitEnemy;

    // Modified Initialize method to accept WeaponStats
    public void Initialize(Vector3 direction, float speed, IStatController damageSource, float killTime = 0)
    {
        
        _direction = direction;
        _damage = damageSource.GetDamage();
        _speed = speed;
        impactDestroyTime = killTime;
        StartCoroutine(SendBulletFlying());

        Destroy(gameObject, destroyTime);
    }

    private IEnumerator SendBulletFlying()
    {
        while (true)
        {
            transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            var enemy = collider.gameObject.GetComponent<EnemyCombatController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(_damage);
                // Fire the OnBulletHitEnemy event
                OnBulletHitEnemy?.Invoke(collider.gameObject);
                Destroy(gameObject, impactDestroyTime);
            }
        }
    }

}
