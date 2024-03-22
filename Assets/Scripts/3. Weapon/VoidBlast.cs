using System.Collections;
using UnityEngine;

public class WeaponWithVoidBlast : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public GameObject explosionEffectPrefab; 
    public float projectileSpeed = 10f;
    public float maxProjectileDistance = 2f; 

    private Vector2 lastMoveDirection;
    private PlayerStatsController playerStatsController;
    private WeaponStats _weaponStats;

    void Start()
    {
        var grandParent = transform.parent.parent;
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        _weaponStats = GetComponent<WeaponStats>();
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown()); 
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        if (playerStatsController != null)
        {
            lastMoveDirection = playerStatsController.GetLastMoveDirection().normalized;
            if (lastMoveDirection != Vector2.zero) 
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position + new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0) * 1.5f, Quaternion.identity); // Spawn projectile in front of player
                StartCoroutine(ProjectileBehaviorRoutine(projectile, lastMoveDirection));
            }
        }
    }

    IEnumerator ProjectileBehaviorRoutine(GameObject projectile, Vector2 direction)
    {
        Vector3 launchPosition = projectile.transform.position;
        float distanceTraveled = 0;

        while (distanceTraveled < maxProjectileDistance)
        {
            projectile.transform.Translate(new Vector3(direction.x, direction.y, 0) * (projectileSpeed * Time.deltaTime), Space.World);
            distanceTraveled += projectileSpeed * Time.deltaTime;
            yield return null;
        }

        Explode(projectile);
    }

    void Explode(GameObject projectile)
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, projectile.transform.position, Quaternion.identity);
            Destroy(explosionEffect, explosionEffect.GetComponent<ParticleSystem>().main.duration); 
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, 4f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Debug.Log("Enemy hit: " + hitCollider.name);
                hitCollider.GetComponent<EnemyCombatController>().EnemyTakeDamage(_weaponStats.GetDamage());
            }
        }
        Destroy(projectile);
    }

}
