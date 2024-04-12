using System.Collections;
using UnityEngine;

public class VoidBlast : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public GameObject explosionEffectPrefab; 

    private Vector2 lastMoveDirection;
    private PlayerStatsController playerStatsController;
    private WeaponStats _weaponStats;

    public Vector2 projectileScale;

    void Start()
    {
        projectileScale = new Vector2(1, 1); //Default projectile scale but talents (such as the AngryKitten talent) will increase this
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

        while (distanceTraveled < _weaponStats.GetAttackRange())
        {
            projectile.transform.Translate(new Vector3(direction.x, direction.y, 0) * (_weaponStats.GetProjectileSpeed() * Time.deltaTime), Space.World);
            distanceTraveled += _weaponStats.GetProjectileSpeed() * Time.deltaTime;
            yield return null;
        }

        Explode(projectile);
    }

    void Explode(GameObject projectile)
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, projectile.transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = projectileScale; // Example scale setting, adjust as needed
            Destroy(explosionEffect, explosionEffect.GetComponent<ParticleSystem>().main.duration); 
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, 4f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                //Debug.Log("Enemy hit: " + hitCollider.name);
                hitCollider.GetComponent<EnemyCombatController>().EnemyTakeDamage(_weaponStats.GetDamage());
            }
        }
        Destroy(projectile);
    }

}
