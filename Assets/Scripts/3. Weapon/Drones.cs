using System.Collections;
using UnityEngine;

public class Drones : MonoBehaviour
{
    private WeaponStats _weaponStats;
    private LineRenderer _lineRenderer;
    private NearestEnemyFinder _nearestEnemyFinder;
    private float _damage;
    [SerializeField]private AudioSource audioSource;
    private int arrayMax;
    private int soundToPlay;
    [SerializeField] private Material laserMaterial;
    [SerializeField] private AudioClip[] arraySounds;
    
    //Talent variables
    public bool shockSphereEnabled;


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
        StartCoroutine(StartFiringAfterDelay());
    }
    
    private IEnumerator StartFiringAfterDelay()
    {
        yield return new WaitForSeconds(1f); // Delay the start of firing by 1 second
        StartCoroutine(FireLaserAutomatically());
    }

    private IEnumerator FireLaserAutomatically()
    {
        while (true)
        {
            FireLaser();
            FadeOutLaser();
            yield return new WaitForSeconds(_weaponStats.GetAttackCooldown());
        }
    }

    private void FireLaser()
    {
        Vector3 dronePosition = transform.position;
        GameObject nearestEnemy = _nearestEnemyFinder.GetNearestEnemy(dronePosition);

        if (nearestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(dronePosition, nearestEnemy.transform.position);

            if (distanceToEnemy <= _weaponStats.GetAttackRange())
            {
                // Laser hits the enemy
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, dronePosition);
                _lineRenderer.SetPosition(1, nearestEnemy.transform.position);

                // Damage the enemy
                EnemyCombatController enemy = nearestEnemy.GetComponent<EnemyCombatController>();
                if (enemy != null)
                {
                    enemy.EnemyTakeDamage(_weaponStats.GetDamage());
                    arrayMax = arraySounds.Length;
                    soundToPlay = Random.Range(0, arrayMax);
                    audioSource.clip = arraySounds[soundToPlay];
                    audioSource.Play();
                }
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }
        else
        {
            _lineRenderer.enabled = false;
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