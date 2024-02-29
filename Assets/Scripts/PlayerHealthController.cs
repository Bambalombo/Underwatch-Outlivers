using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private StatusBarController healthbarController;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbarController.UpdateStatusBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            // Handle player death here
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            // Assume the enemy deals a fixed amount of damage
            TakeDamage(10f);
        }
    }
}