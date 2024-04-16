using System.Collections;
using UnityEngine;

public class MeleeSlash : MonoBehaviour
{
    //TODO Kan godt være det her script skal ændres på et tidspunkt hvis vi ender med at lave sådan at player orientation ændrer sig med movement, tror kinda det vil være bedre men idk
    private WeaponStats _weaponStats;

    public float attackAngle;
    private LineRenderer lineRenderer;
    private float lineVisibilityDuration = 0.5f; // Duration for which the line remains visible after an attack

    private PlayerStatsController playerStatsController;
    private PlayerHealthController playerHealthController;
    private Vector2 lastMoveDirection = Vector2.right; // Default direction if the player has not moved
    
    //Talent variables
    //Bloodtype00 
    public bool bloodType00Enabled;
    

    void Start()
    {
        // Get the WeaponStats component
        _weaponStats = GetComponent<WeaponStats>();
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.05f;
        lineRenderer.positionCount = 3;
        lineRenderer.enabled = false; // Initially hide the line

        var grandParent = transform.parent.parent;
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        playerHealthController = grandParent.GetComponent<PlayerHealthController>();

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
            PerformAttack();
            StartCoroutine(ShowLineRendererTemporarily());
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _weaponStats.GetAttackRange());

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject != this.gameObject)
            {
                Vector2 directionToTarget = (hit.transform.position - transform.position).normalized;
                float angleToTarget = Vector2.Angle(lastMoveDirection, directionToTarget);

                if (angleToTarget < attackAngle / 2)
                {
                    EnemyCombatController enemy = hit.gameObject.GetComponent<EnemyCombatController>();
                    if (enemy != null)
                    {
                        // Deal damage to the enemy
                        enemy.EnemyTakeDamage(_weaponStats.GetDamage());
                        playerHealthController.PlayerHeal(_weaponStats.GetLifeStealAmount());

                        if (bloodType00Enabled)
                        {
                            this.gameObject.GetComponent<AoeDamagePool>().AttemptInitialize(_weaponStats.GetDamage()/10, hit.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }

    IEnumerator ShowLineRendererTemporarily()
    {
        UpdateLineRenderer();
        lineRenderer.enabled = true; // Make the line visible
        yield return new WaitForSeconds(lineVisibilityDuration); // Wait for half a second
        lineRenderer.enabled = false; // Hide the line again
    }

    void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            // Ensure we have a direction to face even when standing still
            Vector3 forwardDirection = new Vector3(lastMoveDirection.x, lastMoveDirection.y, 0);
            Vector3 rightBoundary = Quaternion.Euler(0, 0, attackAngle / 2) * forwardDirection * _weaponStats.GetAttackRange();
            Vector3 leftBoundary = Quaternion.Euler(0, 0, -attackAngle / 2) * forwardDirection * _weaponStats.GetAttackRange();

            lineRenderer.positionCount = 4; // Update to 4 points to form a closed triangle
            lineRenderer.SetPosition(0, transform.position + rightBoundary); // Right vertex of the triangle
            lineRenderer.SetPosition(1, transform.position); // Apex of the triangle (player's position)
            lineRenderer.SetPosition(2, transform.position + leftBoundary); // Left vertex of the triangle
            lineRenderer.SetPosition(3, transform.position + rightBoundary); // Back to the right vertex to close the triangle
        }
    }

}
