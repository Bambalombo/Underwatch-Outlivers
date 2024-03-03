using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private StatusBarController healthBarController;
    private TextMeshProUGUI _deathText;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float enemyDamage = 9f;


    private void Awake()
    {
        currentHealth = maxHealth;
        _deathText = GameObject.FindWithTag("DeathTextTag").GetComponent<TextMeshProUGUI>();
        _deathText.enabled = false;
    }
    
    //TODO: Death needs to be when every player is dead, not just one 
    private void PlayerTakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBarController.UpdateStatusBar(currentHealth, maxHealth);
        if (currentHealth < 0) 
        {
            _deathText.enabled = true;
            _deathText.text = "You Died!";
            // Handle player death here
        }
    }

    // Take damage when colliding with an enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null && enemy.CanAttack())
        {
            // Assume the enemy deals a fixed amount of damage
            PlayerTakeDamage(enemyDamage);
            enemy.Attack();
        }
    }
}