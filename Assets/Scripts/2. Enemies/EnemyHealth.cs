using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject damagePopupPrefab;
    //[SerializeField] private int experienceGain = 10;
    private float _lastAttackTime;
    private Transform _damagePopupParent;
    [SerializeField] private GameObject experiencePickupPrefab;
    private Transform _experiencePickupParent;

    
    //TODO: Too many random variables in this script, need to clean up/move to other scripts

    private void Awake()
    {
        // TODO: Can be optimized but i don't have the energy to fix it right now
        _damagePopupParent = GameObject.FindWithTag("DamagePopupParent").transform;
        _experiencePickupParent = GameObject.FindWithTag("ExperiencePickupParent").transform;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        _lastAttackTime = -attackCooldown;
    }

    public void EnemyTakeDamage(float damage)
    {
        currentHealth -= damage;
        
        KillEnemyAndGivePlayerExp();

        InstantiateDamagePopup(damage);
    }

    private void KillEnemyAndGivePlayerExp()
    {
        if (currentHealth <= 0)
        {
            InstantiateExperiencePickup();
        }
    }
    
    private void InstantiateExperiencePickup()
    {

        Instantiate(experiencePickupPrefab, transform.position, Quaternion.identity, _experiencePickupParent.transform);
        
        
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
        return Time.time >= _lastAttackTime + attackCooldown;
    }

    public void Attack()
    {
        _lastAttackTime = Time.time;
    }
}