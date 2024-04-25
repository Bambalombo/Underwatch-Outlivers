using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCombatController : MonoBehaviour 
{
    [SerializeField] private GameObject damagePopupPrefab;
    private Transform _damagePopupParent; 
    private Transform _experiencePickupParent;
    [SerializeField] private EnemyStatsController enemyStatsController;
    private EnemySpawner _enemySpawner;
    [SerializeField] private GameObject expParticleEffectPrefab;
    
    private void Awake()
    {
        _damagePopupParent = GameManager.GetDamagePopupParent();
        _experiencePickupParent = GameManager.GetExperiencePickupParent();
        _enemySpawner = GameManager.GetSpawnerEnemyControllerParent().GetComponent<EnemySpawner>();
    }


    public void EnemyTakeDamage(float damage)
    {
        int damageAsInt = Mathf.RoundToInt(damage); // Convert the float damage to an int
        
        enemyStatsController.SetCurrentHealth(enemyStatsController.GetCurrentHealth() - damageAsInt);
        
        KillEnemyAndGivePlayerExp();

        InstantiateDamagePopup(damageAsInt);
    }

    private void KillEnemyAndGivePlayerExp()
    {
        if (enemyStatsController.GetCurrentHealth() <= 0)
        {
            InstantiateExperiencePickup();
        }
    }
    
    private void InstantiateExperiencePickup()
    {
        if (enemyStatsController.GetIsBoss())
        {
            BossExperienceDrop();
        }
        else
        {
            // Instantiate the experience drop
            for (int i = 0; i < enemyStatsController.GetExperienceDropAmount(); i++)
                Instantiate(enemyStatsController.GetExperienceDrop(), transform.position, Quaternion.identity, _experiencePickupParent.transform);
        }
        
        float randomFloat = Random.Range(0, 100); // Random number between 0 and 100
        if (randomFloat < enemyStatsController.GetHealthDropChance()) // Chance for health drop
        {
            // Instantiate the health pickup
            Instantiate(enemyStatsController.GetHealthPickup(), transform.position, Quaternion.identity, _experiencePickupParent.transform);
        }
        
        Destroy(gameObject); // Destroy the enemy object
    }
    
    private void BossExperienceDrop()
    {
        if (enemyStatsController.GetIsBoss())
            SoundManager.PlaySound("BossDeathSound");
        Vector3 bossPosition = transform.position;
        // Play the particle effect at the boss position
        var expParticleEffect = Instantiate(expParticleEffectPrefab, bossPosition, Quaternion.identity, _experiencePickupParent);
        Destroy(expParticleEffect, 5f);
        
        float explosionRadius = 5.0f; // Radius for the explosion effect
        for (int i = 0; i < enemyStatsController.GetExperienceDropAmount(); i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            Vector3 spawnPosition = bossPosition + new Vector3(Mathf.Cos(angle) * explosionRadius, Mathf.Sin(angle) * explosionRadius, 0);
            GameObject expDrop = Instantiate(enemyStatsController.GetExperienceDrop(), spawnPosition, Quaternion.identity, _experiencePickupParent);
            Rigidbody2D rb = expDrop.AddComponent<Rigidbody2D>(); // Add Rigidbody
            rb.gravityScale = 0; // Disable gravity
            rb.drag = 1; // Adjust this value to get the desired slowdown effect
            rb.AddForce((spawnPosition - bossPosition).normalized * Random.Range(100, 200)); // Add force to simulate explosion
        }
        
    }

    private void InstantiateDamagePopup(float damage)
    {
        // Disable if no offset is needed
        var randomOffset = RandomOffsetForDamagePopup();

        // Apply the offset to the damage popup's position
        Vector3 popupPosition = transform.position + new Vector3(0, 1, 0) + randomOffset;
        var popup = Instantiate(damagePopupPrefab, popupPosition, Quaternion.identity, _damagePopupParent);
        popup.GetComponent<DamagePopupController>().SetDamage(damage);
    }

    private static Vector3 RandomOffsetForDamagePopup()
    {
        // Generate a random offset
        float randomXOffset = Random.Range(-0.5f, 0.5f); 
        float randomYOffset = Random.Range(0f, 0.5f); 
        Vector3 randomOffset = new Vector3(randomXOffset, randomYOffset, 0);
        return randomOffset;
    }

    public bool CanAttack()
    {
        return Time.time >= enemyStatsController.GetLastAttackTime() + enemyStatsController.GetAttackCooldown();
    }

    public void Attack()
    {
        enemyStatsController.SetLastAttackTime(Time.time);
    }
    
    private void OnDestroy()
    {
        _enemySpawner.RemoveEnemyFromList(gameObject);
        
    }
}