using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCombatController : MonoBehaviour 
{
    [SerializeField] private GameObject damagePopupPrefab;
    private Transform _damagePopupParent; 
    private Transform _experiencePickupParent;
    [SerializeField] private EnemyStatsController enemyStatsController;
    private SpawnerEnemyController _spawnerEnemyController;
    
    private void Awake()
    {
        _damagePopupParent = GameManager.GetDamagePopupParent();
        _experiencePickupParent = GameManager.GetExperiencePickupParent();
        _spawnerEnemyController = GameManager.GetSpawnerEnemyControllerParent().GetComponent<SpawnerEnemyController>();
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

        Instantiate(enemyStatsController.GetExperienceDrop(), transform.position, 
            Quaternion.identity, _experiencePickupParent.transform);
        
        
        Destroy(gameObject); // Destroy the enemy object
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
        _spawnerEnemyController.RemoveEnemyFromList(gameObject);
        
    }
}