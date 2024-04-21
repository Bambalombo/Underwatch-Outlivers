using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyLitterPrefabScript : MonoBehaviour
{
    public float speed = 5.0f;  // Speed at which the minion moves towards the enemy
    public GameObject explosionPrefab;

    private AbilityStats _abilityStats;
    private WeaponStats _weaponStats;
    private NearestEnemyFinder _enemyFinder;
    private GameObject target;  // Current target enemy

    // Start is called before the first frame update
    void Start()
    {
        _enemyFinder = FindObjectOfType<NearestEnemyFinder>();  // Make sure there is an NearestEnemyFinder in the scene
        target = _enemyFinder.GetNearestEnemy(transform.position);
    }

    public void Initialize(AbilityStats abilityStats, NearestEnemyFinder nearestEnemyFinder, WeaponStats weaponStats)
    {
        _enemyFinder = nearestEnemyFinder;
        _abilityStats = abilityStats;
        _weaponStats = weaponStats;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            target = _enemyFinder.GetNearestEnemy(transform.position);  // Continuously find a new target if the previous one is null
        }
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))  // Check if the colliding object has the "Enemy" tag
        {
            Debug.Log("I am EXPLODING AHHHH");
            Explode();
            Destroy(gameObject);  // Destroy the minion after explosion
        }
    }

    void Explode()
    {
        GameObject explosionEffect = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionEffect.transform.localScale = new Vector3(_weaponStats.GetAttackRange(), _weaponStats.GetAttackRange(), 0); // Example scale setting, adjust as needed
        Destroy(explosionEffect, explosionEffect.GetComponent<ParticleSystem>().main.duration); 
        // This will detect all colliders within the explosion radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _weaponStats.GetAttackRange());
        foreach (Collider2D enemy in hitColliders)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<EnemyCombatController>().EnemyTakeDamage(_weaponStats.GetDamage() * 2f);
            }
        }
    }


}
