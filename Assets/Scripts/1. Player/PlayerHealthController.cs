using UnityEngine;
using TMPro;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] private StatusBarController healthBarController;
    private TextMeshProUGUI _deathText;
    private PlayerStatsController _playerStatsController;
    

    private void Awake()
    {
        _deathText = GameObject.FindWithTag("DeathTextTag").GetComponent<TextMeshProUGUI>();
        _deathText.enabled = false;
        
        _playerStatsController = GetComponent<PlayerStatsController>();
    }
    
    //TODO: Death needs to be when every player is dead, not just one 
    public void PlayerTakeDamage(float damage)
    {
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetCurrentHealth() - damage);
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
        if (_playerStatsController.GetCurrentHealth() <= 0) 
        {
            _deathText.enabled = true;
            _deathText.text = "You Died!";
            // Handle player death here
        }
    }
    
    public void PlayerHeal(float healAmount)
    {
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetCurrentHealth() + healAmount);
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
    }

    // Take damage when colliding with an enemy
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null && enemy.CanAttack())
        {
            // Assume the enemy deals a fixed amount of damage
            PlayerTakeDamage(enemyDamage);
            enemy.Attack();
        }
    }*/
    
    // Take damage when staying in an enemy's trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy")) return;
        
        var enemyHealth = collision.gameObject.GetComponent<EnemyCombatController>();
        
        if (!enemyHealth.CanAttack()) return; // If the enemy can't attack, return
        
        var enemyStats = collision.gameObject.GetComponent<EnemyStatsController>();
        PlayerTakeDamage(enemyStats.GetDamage());
        enemyHealth.Attack();
    }
}