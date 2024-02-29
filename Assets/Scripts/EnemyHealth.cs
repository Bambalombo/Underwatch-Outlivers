using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    private float lastAttackTime;
    [SerializeField] private float attackCooldown = 1f; // 1 second cooldown

    private void Start()
    {
        currentHealth = maxHealth;
        lastAttackTime = -attackCooldown; // Initialize to a value that allows immediate attack
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the enemy if health is 0 or less
        }
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