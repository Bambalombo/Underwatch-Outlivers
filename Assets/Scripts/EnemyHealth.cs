using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private float lastAttackTime;
    [SerializeField] private float attackCooldown = 1f; 
    [SerializeField] private GameObject damagePopupPrefab;


    private void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        Vector3 popupPosition = transform.position + new Vector3(0, 1, 0); // Adjust the Y offset as needed
        var popup = Instantiate(damagePopupPrefab, popupPosition, Quaternion.identity);
        popup.GetComponent<DamagePopupController>().SetDamage(damage);
    }

    public bool CanAttack()
    {
        return Time.time >= lastAttackTime + attackCooldown;
    }

    public void Attack()
    {
        lastAttackTime = Time.time;
    }
}