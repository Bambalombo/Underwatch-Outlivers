using UnityEngine;
using TMPro;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private StatusBarController healthbarController;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float enemyDamage = 9f;


    private void Start()
    {
        currentHealth = maxHealth;
        deathText = GameObject.FindWithTag("DeathTextTag").GetComponent<TextMeshProUGUI>();
        deathText.enabled = false;
    }
    
    //TODO: Death needs to be when every player is dead, not just one 
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthbarController.UpdateStatusBar(currentHealth, maxHealth);
        if (currentHealth < 0) 
        {
            deathText.enabled = true;
            deathText.text = "You Died!";
            // Handle player death here
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            // Assume the enemy deals a fixed amount of damage
            TakeDamage(enemyDamage);
        }
    }
}