using System;
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
    [SerializeField] private bool _isAlive = true;
    private Coroutine _respawnPlayerCoroutine;
    
    [Header("Player Taking Damage")]
    [SerializeField] private Color hurtColor = Color.red; 
    [SerializeField] private Color healColor = Color.green;
    [SerializeField] private float colorRecoverTime = 0.25f;
    private Color _originalColor;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _flashSpriteColorCoroutine;
    
    [Header("Script References")]
    private PlayerStatsController _playerStatsController;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private DeathIconTimer deathIconTimer;

    
    private void Start()
    {
        _playerStatsController = GetComponent<PlayerStatsController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        deathIconTimer = GameObject.FindWithTag("DeathIcons").GetComponent<DeathIconTimer>();
    }

    private void OnEnable()
    {
        GameManager.OnPlayerRespawn += RespawnPlayer;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerRespawn -= RespawnPlayer;
        StopAllCoroutines();
        _spriteRenderer.color = _originalColor;
    }

    public bool IsAlive()
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
            TryKillPlayer();
            return;
        }
        
        if (_flashSpriteColorCoroutine != null)
            StopCoroutine(_flashSpriteColorCoroutine);
        
        _flashSpriteColorCoroutine = StartCoroutine(FlashSpriteColor(hurtColor));
    }

    public void TryKillPlayer()
    {
        Debug.Log("Trying to kill player");
        if (!_isAlive)
            return;
            
        _isAlive = false;
            
        bool anyPlayerAlive = GameManager.GetPlayerHealthControllers().Any(player => player.IsAlive());
        if (!anyPlayerAlive && gameManager.EndGameEnabled())
            gameManager.StartGameOverSequence();

        if (canDie)
        {
            Debug.Log("Killing player.");
            gameManager.PlayerDied(_playerStatsController.GetPlayerIndex(), _playerStatsController.GetRespawnTime());
        }
    }
    
    public void PlayerHeal(float healAmount)
    {
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetCurrentHealth() + healAmount);
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
        _flashSpriteColorCoroutine = StartCoroutine(FlashSpriteColor(healColor));
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BossWeapon"))
        {
            var damage = collision.gameObject.GetComponent<BossBullet>().GetDamage();
            
            PlayerTakeDamage(damage);
            
            Destroy(collision.gameObject);
        }
    }

    private void RespawnPlayer(int playerIndex)
    {
        if (playerIndex != _playerStatsController.GetPlayerIndex())
            return;
        
        _isAlive = true;
        _playerStatsController.SetCurrentHealth(_playerStatsController.GetMaxHealth());
        healthBarController.UpdateStatusBar(_playerStatsController.GetCurrentHealth(), _playerStatsController.GetMaxHealth());
    }
    
    private IEnumerator FlashSpriteColor(Color flashColor)
    {
        _spriteRenderer.color = flashColor; 
        
        float duration = colorRecoverTime;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _spriteRenderer.color = Color.Lerp(flashColor, _originalColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        _spriteRenderer.color = _originalColor; 
    }

    public void SetAlive(bool value)
    {
        _isAlive = value;
    }
}