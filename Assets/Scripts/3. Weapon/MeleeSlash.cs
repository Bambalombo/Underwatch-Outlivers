using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeSlash : MonoBehaviour
{
    //TODO Kan godt være det her script skal ændres på et tidspunkt hvis vi ender med at lave sådan at player orientation ændrer sig med movement, tror kinda det vil være bedre men idk
    private WeaponStats _weaponStats;

    public float attackAngle;
    private LineRenderer lineRenderer;
    private float lineVisibilityDuration = 0.5f; // Duration for which the line remains visible after an attack

    private GameObject _playerGameObject;
    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private Vector2 lastMoveDirection = Vector2.right; // Default direction if the player has not moved

    public GameObject meleeSlashPrefab;
    
    //Talent variables
    //Bloodtype00 
    public bool bloodType00Enabled;
    public bool healingHackEnabled;
    public float healingHackRange;
    
    [SerializeField]private AudioSource audioSource;
    [SerializeField] private AudioClip[] arraySounds;
    private int arrayMax;
    private int soundToPlay;

    private void Awake()
    {
        _playerGameObject = transform.parent.parent.gameObject;
        playerStatsController = _playerGameObject.GetComponent<PlayerStatsController>();
        playerHealthController = _playerGameObject.GetComponent<PlayerHealthController>();
        _weaponStats = GetComponent<WeaponStats>();
        
        arrayMax = arraySounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
    }

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = false; // Initially hide the line
    }

    private void OnEnable()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
            PerformAttack();
            //StartCoroutine(ShowLineRendererTemporarily());
        }
    }

    void Update()
    {
        Vector2 currentMoveDirection = playerStatsController.GetLastMoveDirection();
        if (currentMoveDirection != Vector2.zero)
        {   
            lastMoveDirection = currentMoveDirection.normalized; // Normalize to ensure the direction vector has a consistent length
        }
    }

    void PerformAttack()
    {
        // Play a random attack sound
        arrayMax = arraySounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
        audioSource.Play();

        // Get the last movement direction
        Vector2 lastMoveDirection = playerStatsController.GetLastMoveDirection();
        if (lastMoveDirection == Vector2.zero) // In case the player hasn't moved, use a default direction
            lastMoveDirection = Vector2.right; // Default direction is right, change as needed

        // Calculate the position in front of the player based on last move direction
        Vector3 spawnPosition = transform.position + new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0) * (_weaponStats.GetAttackRange() * 0.75f); // Multiply by a larger factor to move it further

        // Calculate the rotation based on the last movement direction
        float angle = Mathf.Atan2(lastMoveDirection.y, lastMoveDirection.x) * Mathf.Rad2Deg;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, angle);

        // Instantiate melee attack effect prefab
        GameObject explosionEffect = Instantiate(meleeSlashPrefab, spawnPosition, spawnRotation);
        explosionEffect.transform.localScale = new Vector3(_weaponStats.GetAttackRange(), _weaponStats.GetAttackRange(), 1); // Scale the effect
        Destroy(explosionEffect, 2); // Destroy effect after 1 second, adjust as needed

        // Detect enemies within the attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(spawnPosition, _weaponStats.GetAttackRange() / 2f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) // Ensure you're only hitting objects tagged as "Enemy"
            {
                EnemyCombatController enemyController = hitCollider.GetComponent<EnemyCombatController>();
                if (enemyController != null)
                {
                    enemyController.EnemyTakeDamage(_weaponStats.GetDamage()); // Damage the enemy
                    playerHealthController.PlayerHeal(_weaponStats.GetLifeStealAmount());
                    
                    if (bloodType00Enabled)
                    {
                        this.gameObject.GetComponent<AoeDamagePool>().AttemptInitialize(_weaponStats.GetDamage() / 10,
                            hitCollider.gameObject.transform.position);
                    }
                    
                    if (healingHackEnabled)
                    {
                        GameObject[] playerGameObjects = GameManager.GetPlayerGameObjects();
                        foreach (GameObject player in playerGameObjects)
                        {
                            float distance = Vector3.Distance(_playerGameObject.transform.position,
                                player.transform.position);

                            if (player != _playerGameObject &&
                                distance <=
                                healingHackRange) // Check if the player is within 5 units and is not the caster
                            {
                                PlayerHealthController healthController = player.GetComponent<PlayerHealthController>();
                                if (healthController != null) // Check if the component exists
                                {
                                    healthController.PlayerHeal(_weaponStats.GetLifeStealAmount() / 10);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}
