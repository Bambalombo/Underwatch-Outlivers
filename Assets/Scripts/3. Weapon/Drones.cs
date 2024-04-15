using System.Collections;
using UnityEngine;

public class Drones : MonoBehaviour
{
    private WeaponStats _weaponStats;
    private LineRenderer _lineRenderer;
    private NearestEnemyFinder _nearestEnemyFinder;
    private float _damage;
    [SerializeField] private Material laserMaterial;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>(); 
        _lineRenderer.widthMultiplier = 0.1f;
        _lineRenderer.positionCount = 2;
        _lineRenderer.material = laserMaterial;

        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        
    }
    
    public void GetWeaponStats(WeaponStats weaponStats)
    {
        _weaponStats = weaponStats;
        
        StartFiring();
    }

    private void StartFiring()
    {
        StartCoroutine(FireLaserAutomatically());
    }

    private IEnumerator FireLaserAutomatically()
    {
        yield return new WaitForSeconds(0.5f);
        FireLaser();
        
        while (true)
        {
            FireLaser();
            FadeOutLaser();
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
        }
    }

    private void FireLaser() //TODO: THE LASER DOES NOT SHOOT INSTANTLY FOR SOME REASON!!!
    {
        Vector3 dronePosition = transform.position;
        GameObject nearestEnemy = _nearestEnemyFinder.GetNearestEnemy(dronePosition);

        if (nearestEnemy != null)
        {
            Vector3 directionToEnemy = (nearestEnemy.transform.position - transform.position).normalized;
            float distanceToEnemy = Vector3.Distance(dronePosition, nearestEnemy.transform.position);

            if (distanceToEnemy > _weaponStats.GetAttackRange())
            {
                _lineRenderer.enabled = false;
                return;
            }

            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, dronePosition);

            RaycastHit2D hit = Physics2D.Raycast(dronePosition, directionToEnemy, _weaponStats.GetAttackRange());

            Vector3 endPosition;
            if (hit.collider != null)
            {
                endPosition = hit.point;

                EnemyCombatController enemy = hit.collider.GetComponent<EnemyCombatController>();
                if (enemy != null)
                {
                    enemy.EnemyTakeDamage(_weaponStats.GetDamage());
                }
            }
            else
            {
                endPosition = dronePosition + directionToEnemy * _weaponStats.GetAttackRange();
            }

            _lineRenderer.SetPosition(1, endPosition);
        }
    }

    private void FadeOutLaser()
    {
        StartCoroutine(FadeOutLaserCoroutine());
    }

    private IEnumerator FadeOutLaserCoroutine()
    {
        float fadeDuration = _weaponStats.GetAttackCooldown()/2;
        float fadeSpeed = 1 / fadeDuration;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            Color color = _lineRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, t * fadeSpeed);
            _lineRenderer.material.color = color;

            yield return null;
        }

        Color resetColor = _lineRenderer.material.color;
        resetColor.a = 1;
        _lineRenderer.material.color = resetColor;

        _lineRenderer.enabled = false;
    }
}