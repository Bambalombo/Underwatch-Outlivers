using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightning : MonoBehaviour
{
    [SerializeField] private AbilityStats stats;
    [SerializeField] private Material lightningMaterial;
    [SerializeField] private float lightningJumpDelay;
    [SerializeField] private float damageAmplifier = 1.5f;
    [SerializeField] private GameObject rangeIndicator;

    private float _extraTargets;
    private NearestEnemyFinder _nearestEnemyFinder;
    private List<GameObject> _enemyHitList;
    private Vector3 _lastPos;
    private Vector3 _playerPosition;
    private Coroutine _windUpTimerCoroutine;
    private Renderer _rangeIndicatorRenderer;
    private Animator _a;
    private ParticleSystem ps;

    private void Awake()
    {
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        _a = GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        rangeIndicator = transform.parent.parent.gameObject.transform.Find("Range Indicator").gameObject;
        rangeIndicator.transform.localScale = new Vector3(stats.GetAttackRange() * 2, stats.GetAttackRange() * 2, 1f);
        
        _rangeIndicatorRenderer = rangeIndicator.GetComponent<Renderer>();
        
        ParticleSystem.MainModule main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        SetSizeOverLifetime();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _windUpTimerCoroutine = StartCoroutine(WindUpLightningCoroutine());
        }
        
        if (Input.GetKeyUp(KeyCode.L))
        {
            StopCoroutine(_windUpTimerCoroutine);
            _playerPosition = gameObject.transform.parent.position;

            if (NearestEnemyIsInRange())
                StartChainLightning();
            
            // rangeIndicator.SetActive(false);
        }
    }

    private IEnumerator WindUpLightningCoroutine()
    {
        _extraTargets = 0;
        
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1f);
            // perform some animation here
            _extraTargets++;
            Debug.Log($"Extra targets to be hit: {_extraTargets}, for a total of {stats.GetTargetCount()+_extraTargets} enemies.");
        }
    }

    private IEnumerator DecideRangeIndicatorColour()
    {
        for (;;)
        {
            if (NearestEnemyIsInRange())
            {
                _rangeIndicatorRenderer.material.color = new Color(0f, 1f, 0f, 0.25f);
            }
            else
            {
                _rangeIndicatorRenderer.material.color = new Color(1f, 0f, 0f, 0.25f);
            }
            
            Debug.Log("Check performed");
            yield return null;
        }
    }

    private bool NearestEnemyIsInRange()
    {
        var nearestEnemy = _nearestEnemyFinder.GetNearestEnemy(_playerPosition);
        if (nearestEnemy == null) return false;
        var distance = Vector3.Distance(_playerPosition, nearestEnemy.transform.position);
        return distance < stats.GetAttackRange();
    }

    private void StartChainLightning()
    {
        StartCoroutine(StartChainLightningCoroutine());
    }

    private void SetSizeOverLifetime()
    {
        var size = ps.sizeOverLifetime;
        size.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, 0.0f);
        curve.AddKey(0.1f, 1.0f);
        curve.AddKey(1.0f, 0.0f);
        size.size = new ParticleSystem.MinMaxCurve(1.0f, curve);
    }
    
    private IEnumerator StartChainLightningCoroutine()
    {
        ps.transform.position = _playerPosition;
        Vector3 lastPos = _playerPosition;
        _enemyHitList = _nearestEnemyFinder.GetChainOfEnemies(_playerPosition, stats.GetTargetCount() + _extraTargets);

        for (int i = 0; i < _enemyHitList.Count; i++)
        {
            if (_enemyHitList[i] == null) continue;
            
            var enemyPos = _enemyHitList[i].transform.position;
            StartCoroutine(DrawLightning(lastPos, enemyPos));
            lastPos = enemyPos;

            if (_enemyHitList[i] != null && _enemyHitList[i].TryGetComponent<EnemyCombatController>(out var enemyCombatController))
            {
                enemyCombatController.EnemyTakeDamage(stats.GetDamage() * (float)Math.Pow(damageAmplifier, _extraTargets));
            }

            yield return new WaitForSeconds(lightningJumpDelay);
        }

        yield return new WaitForSeconds(1f);
        ps.transform.position = _playerPosition;
        
        yield return null;
    }

    private IEnumerator DrawLightning(Vector3 startPos, Vector3 endPos)
    {
        ps.Play();
        
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startSize = 0.2f + (_extraTargets / 5f);
        
        emitParams.position = startPos;
        ps.Emit(emitParams, 1);

        yield return new WaitForSeconds(lightningJumpDelay);

        emitParams.position = endPos;
        ps.Emit(emitParams, 1);
        
        ps.transform.position = startPos;
        
        yield return null;
    }
}
