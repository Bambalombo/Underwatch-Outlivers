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
    public float travelDistance; 
    
    
    [SerializeField]private AudioSource audioSource;
    [SerializeField] private AudioClip[] arraySounds;
    private int arrayMax;
    private int soundToPlay;

    void Awake()
    {
        projectileScale = new Vector2(1, 1); //Default projectile scale but talents (such as the AngryKitten talent) will increase this
        var grandParent = transform.parent.parent;
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        _weaponStats = GetComponent<WeaponStats>();
        
        arrayMax = arraySounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
    }
    
    private void OnEnable()
    {
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

        while (distanceTraveled < travelDistance)
        {
            projectile.transform.Translate(new Vector3(direction.x, direction.y, 0) * (_weaponStats.GetProjectileSpeed() * Time.deltaTime), Space.World);
            distanceTraveled += _weaponStats.GetProjectileSpeed() * Time.deltaTime;
            yield return null;
        }
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
        audioSource.Play();
        Explode(projectile);
    }

    void Explode(GameObject projectile)
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, projectile.transform.position, Quaternion.identity);
            explosionEffect.transform.localScale = new Vector3(_weaponStats.GetAttackRange(),_weaponStats.GetAttackRange(),0); // Example scale setting, adjust as needed
            Destroy(explosionEffect, explosionEffect.GetComponent<ParticleSystem>().main.duration); 
        }
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(projectile.transform.position, _weaponStats.GetAttackRange()/2f);
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
