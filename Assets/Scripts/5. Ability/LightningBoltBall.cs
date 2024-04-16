using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LightningBoltBall : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private AbilityStats stats;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D circleCollider2D;
    
    private NearestEnemyFinder _nearestEnemyFinder;
    private ParticleSystem ps;
    private List<GameObject> _enemyHitList;
    
    private float _extraTargets = 0f;
    private float _extraForks = 0f;
    private float _damageAmplifier = 1.5f;
    
    private float _lightningJumpDelay = 0.05f;

    private void Start()
    {
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
    }
    
    public void SetBoltPowerLevel(float extraTargets, float extraForks, float damageAmplifier)
    {
        _extraTargets = extraTargets;
        _extraForks = extraForks;
        _damageAmplifier = damageAmplifier;
    }
    
    public void SetAbilityStatsReference(AbilityStats abilityStats)
    {
        stats = abilityStats;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            StartChainLightning(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void StartChainLightning(GameObject enemy)
    {
        StartCoroutine(StartChainLightningCoroutine(enemy));
        circleCollider2D.enabled = false;
        spriteRenderer.enabled = false;
    }
    
    private IEnumerator StartChainLightningCoroutine(GameObject enemy)
    {
        var hitPosition = enemy.transform.position;
        ps.transform.position = hitPosition;
        Vector3 lastPos = hitPosition;
        _enemyHitList = _nearestEnemyFinder.GetChainOfEnemiesInProximity(hitPosition, stats.GetTargetCount() + _extraTargets, stats.GetAttackRange());

        for (int i = 0; i < _enemyHitList.Count; i++)
        {
            if (_enemyHitList[i] == null) continue;
            
            var enemyPos = _enemyHitList[i].transform.position;
            StartCoroutine(DrawLightning(lastPos, enemyPos));
            lastPos = enemyPos;

            if (_enemyHitList[i] != null && _enemyHitList[i].TryGetComponent<EnemyCombatController>(out var enemyCombatController))
            {
                _damageAmplifier = stats.GetTargetCount() / _enemyHitList.Count;
                enemyCombatController.EnemyTakeDamage(stats.GetDamage() * _damageAmplifier);
            }

            yield return new WaitForSeconds(_lightningJumpDelay);
        }

        yield return new WaitForSeconds(1f);
        ps.Stop();
        
        yield return null;
    }
    
    private IEnumerator DrawLightning(Vector3 startPos, Vector3 endPos)
    {
        //ps.Stop();
        //ps.Clear();
        //ps.Play();
        
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startSize = 0.2f + (_extraTargets / 5f);
        
        emitParams.position = startPos;
        ps.Emit(emitParams, 1);

        yield return new WaitForSeconds(_lightningJumpDelay);

        emitParams.position = endPos;
        ps.Emit(emitParams, 1);
        
        emitParams.position = startPos;
        
        yield return null;
    }
}
