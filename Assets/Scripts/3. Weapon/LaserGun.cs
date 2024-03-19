using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour
{
    private WeaponStats _weaponStats;
    private LineRenderer _lineRenderer;
    private NearestEnemyFinder _nearestEnemyFinder;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private Material laserMaterial;
    
    private void Awake()
    {
        // Get the WeaponStats component
        _weaponStats = GetComponent<WeaponStats>();
        
        _lineRenderer = GetComponent<LineRenderer>(); // Get the LineRenderer component
        _lineRenderer.widthMultiplier = 0.1f; // The width of the laser
        _lineRenderer.positionCount = 2; // The LineRenderer has 2 points (start and end)
        _lineRenderer.material = laserMaterial; // Assign the laser material to the LineRenderer

        
        // Get the PlayerStatsController component from the grandparent of the weapon
        var grandParent = transform.parent.parent;
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        
        // Get the NearestEnemyFinder component from the SpawnerEnemyController
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        
        // Start firing the laser automatically
        StartFiring();
    }

    private void StartFiring()
    {
        StartCoroutine(FireLaserAutomatically());
    }

    private IEnumerator FireLaserAutomatically()
    {
        while (true)
        {
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
            FireLaser();
            FadeOutLaser();
        }
    }

    private void FireLaser()
    {
        // Enable the LineRenderer when firing the laser
        _lineRenderer.enabled = true;
        
        Vector3 playerPosition = playerStatsController.GetPlayerPosition(); // Set the start position of the laser
        
        // Set the start position of the laser
        _lineRenderer.SetPosition(0, playerPosition);
        
        // Get the nearest enemy
        GameObject nearestEnemy = _nearestEnemyFinder.GetNearestEnemy(playerPosition);

        // If there is a nearest enemy, fire at it
        if (nearestEnemy != null)
        {
            // Calculate the direction to the nearest enemy
            Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;

            // Calculate the end position of the laser
            Vector3 endPosition = playerPosition + directionToEnemy * _weaponStats.GetAttackRange();

            // Set the end position of the laser
            _lineRenderer.SetPosition(1, endPosition);

            // Check for enemies in the path of the laser
            RaycastHit2D[] hits = Physics2D.LinecastAll(playerPosition, endPosition);
            foreach (RaycastHit2D hit in hits)
            {
                EnemyCombatController enemy = hit.collider.GetComponent<EnemyCombatController>();
                if (enemy != null)
                {
                    // Deal damage to the enemy
                    enemy.EnemyTakeDamage(_weaponStats.GetDamage());
                }
            }
        }
    }

    private void FadeOutLaser()
    {
        StartCoroutine(FadeOutLaserCoroutine());
    }

    private IEnumerator FadeOutLaserCoroutine()
    {
        float fadeDuration = _weaponStats.GetAttackLifetime();
        float fadeSpeed = 1 / fadeDuration;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            // Gradually decrease the alpha of the laser's color
            Color color = _lineRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, t * fadeSpeed);
            _lineRenderer.material.color = color;

            yield return null;
        }

        // Reset the laser's color to fully opaque
        Color resetColor = _lineRenderer.material.color;
        resetColor.a = 1;
        _lineRenderer.material.color = resetColor;

        // Disable the LineRenderer
        _lineRenderer.enabled = false;
    }
}