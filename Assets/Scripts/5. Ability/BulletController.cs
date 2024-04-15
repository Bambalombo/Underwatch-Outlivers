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
    // private WeaponStats _weaponStats; // Store weapon stats
    // private AbilityStats _abilityStats;
    private float _damage;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<EnemyCombatController>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(_damage); // Adjust '10' if needed
                Destroy(gameObject, impactDestroyTime);
            }
        }
    }
}
