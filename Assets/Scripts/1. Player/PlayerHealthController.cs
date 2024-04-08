using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;


public class PlayerHealthController : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private StatusBarController healthBarController;
    [SerializeField] private StatusBarController cooldownBarController;
    
    [Header("Player Death")]
    [SerializeField] private bool canDie = true;
    private TextMeshProUGUI _deathText;
    private bool _isAlive = true;
    private Coroutine _respawnPlayerCoroutine;
    
    [Header("Player Taking Damage")]
    [SerializeField] private Color hurtColor = Color.red; 
    [SerializeField] private float colorRecoverTime = 0.25f;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _flashSpriteColorCoroutine;
    
    // References
    private PlayerStatsController _playerStatsController;
    [SerializeField] private GameManager gameManager;

    
    
    private void Awake()
    {
        _deathText = GameObject.FindWithTag("DeathTextTag").GetComponent<TextMeshProUGUI>();
        _deathText.enabled = false;
        
        _playerStatsController = GetComponent<PlayerStatsController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    private bool CheckIfAlive()
    {
        return _isAlive;
    }
    
    //TODO: Death needs to be when every player is dead, not just one 
    public void PlayerTakeDamage(float damage)
    {
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetCurrentHealth() - damage);
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
       
        if (_playerStatsController.GetCurrentHealth() <= 0)
        {
            if (_respawnPlayerCoroutine != null)
                return;
            
            _isAlive = false;
            bool anyPlayerAlive = GameManager.GetPlayerHealthControllers().Any(player => player.CheckIfAlive());
           
            if (!anyPlayerAlive && gameManager.EndGameEnabled())
                gameManager.StartGameOverSequence();

            KillPlayer();
            return;
        }
        
        if (_flashSpriteColorCoroutine != null)
            StopCoroutine(_flashSpriteColorCoroutine);
        
        _flashSpriteColorCoroutine = StartCoroutine(FlashSpriteColor());
    }
    
    public void PlayerHeal(float healAmount)
    {
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetCurrentHealth() + healAmount);
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
    }
    
    public void UpdateCoolDownBar(float currentCooldown, float maxCooldown)
    {
        cooldownBarController.UpdateStatusBar(currentCooldown, maxCooldown);
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

    private void KillPlayer()
    {
        if (!canDie) return;
        
        _deathText.enabled = true;
        _deathText.text = "You Died!";
        
        _respawnPlayerCoroutine = StartCoroutine(StartRespawnTimer());
    }
    
    private IEnumerator StartRespawnTimer()
    {
        var respawnTime = _playerStatsController.GetRespawnTime();
        
        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1f);
            respawnTime--;
            Debug.Log($"Respawning in {respawnTime} seconds.");
        }
        
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        _isAlive = true;
        _deathText.enabled = false;
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetMaxHealth());
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
    }
    
    private IEnumerator FlashSpriteColor()
    {
        _spriteRenderer.color = hurtColor;
        
        float duration = colorRecoverTime;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _spriteRenderer.color = Color.Lerp(Color.red, _originalColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = _originalColor;
    }
}