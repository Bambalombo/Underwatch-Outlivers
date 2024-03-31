using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

public class LightningBolt : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private AbilityStats stats;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private GameObject handheldBolt;
    [SerializeField] private GameObject flyingBolt;
    
    [FormerlySerializedAs("powerChargeTimeStep")]
    [Header("Gameplay Settings")]
    [SerializeField] private float chargeTimeStep = 1.0f;
    [SerializeField] private float chargeMaxStacks = 5f;
    [SerializeField] private float autoReleaseTime = 3f;
    
    [Header("Visual Settings")]
    [SerializeField] private Material lightningBoltMaterial;
    [SerializeField] private float boltStartSize = 0.8f;
    [SerializeField] private float boltMaxSize = 1.8f;
    
    private Vector3 _playerPosition;
    private Coroutine _chargeLightningBoltCoroutine;
    private Coroutine _updateBoltPositionCoroutine;
    private ParticleSystem ps;
    private float _boltPowerLevel;
    private GameObject _lightningBolt;
    
    [SerializeField] private float defaultCooldown;
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 

    private void Awake()
    {
        var grandParent = transform.parent.parent;    
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
        abilityStats = GetComponent<AbilityStats>();
    }
    
    private void OnAbilityUsed()
    {
        //Det her er lidt ødelægger pointen med at charge den, men kan ikk finde en måde at lave GetKeyUp på med nye input system, sorry Linus ;_; (Ability bliver triggered med Q btw)
        StartChargingLightningBolt();
        ReleaseLightningBolt();
        abilityCastHandler.StartCooldown(defaultCooldown,abilityStats.GetAttackCooldown());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartChargingLightningBolt();
        }
        
        if (Input.GetKeyUp(KeyCode.L) & _chargeLightningBoltCoroutine != null)
        {
            ReleaseLightningBolt();
        }
    }

    private void StartChargingLightningBolt()
    {
        _chargeLightningBoltCoroutine = StartCoroutine(ChargeUpLightningBolt());
    }

    private void ReleaseLightningBolt()
    {
        StopCoroutine(_chargeLightningBoltCoroutine);
        //StopCoroutine(_updateBoltPositionCoroutine);
        _chargeLightningBoltCoroutine = null;
        Vector2 spawnPosition = _lightningBolt.transform.position;
        Destroy(_lightningBolt);
        Vector2 spawnDirection = playerStatsController.GetLastMoveDirection().normalized;
        StartCoroutine(SendBoltFlying(spawnPosition, spawnDirection));
    }

    private IEnumerator ChargeUpLightningBolt()
    {
        var trans = transform;
        Vector2 spawnPosition = (Vector2)trans.position;
        _lightningBolt = Instantiate(handheldBolt, new Vector3(spawnPosition.x + 0.63f, spawnPosition.y + 0.35f, 1), Quaternion.identity, trans);
        _updateBoltPositionCoroutine =  StartCoroutine(UpdateBoltHandPosition(_lightningBolt.transform));
        
        _boltPowerLevel = 0;

        for (var i = 0; i < chargeMaxStacks; i++)
        {
            _boltPowerLevel = i;

            if (_lightningBolt != null)
            {
                var boltSize = boltStartSize + (_boltPowerLevel / chargeMaxStacks) * boltMaxSize;
                _lightningBolt.transform.localScale = new Vector3(boltSize, boltSize, 1);
            }

            yield return new WaitForSeconds(chargeTimeStep);
        }

        yield return new WaitForSeconds(autoReleaseTime);

        if (_lightningBolt != null)
        {
            ReleaseLightningBolt();
        }
    }

    private IEnumerator UpdateBoltHandPosition(Transform boltTransform)
    {
        var playerSprite = transform.parent.parent.GetComponent<SpriteRenderer>();
        
        var xPos = 0.63f;
        var yPos = 0.35f;

        while (boltTransform != null)
        {
            var playerPos = transform.position;
            if (playerSprite.flipX)
            {
                boltTransform.position = new Vector3(playerPos.x - xPos, playerPos.y + yPos, 1);
            }
            else
            {
                boltTransform.position = new Vector3(playerPos.x + xPos, playerPos.y + yPos, 1);
            }
            
            yield return null;
        }

        yield return null;
    }
    
    private IEnumerator SendBoltFlying(Vector2 spawnPosition, Vector2 spawnDirection)
    {
        var flyingBoltInstance = Instantiate(flyingBolt, spawnPosition, Quaternion.identity);
        var flyingBoltController = flyingBoltInstance.GetComponent<LightningBoltBall>();
        Destroy(flyingBoltInstance, 10f);
     
        var flyingBoltSize = boltStartSize + (_boltPowerLevel / chargeMaxStacks) * boltMaxSize;
        flyingBoltInstance.transform.localScale = new Vector3(flyingBoltSize, flyingBoltSize, 1);
        
        flyingBoltController.SetBoltPowerLevel(_boltPowerLevel, 0, 1.5f);
        flyingBoltController.SetAbilityStatsReference(stats);

        while (flyingBoltInstance != null)
        {
            flyingBoltInstance.transform.Translate(spawnDirection * (stats.GetProjectileSpeed() * Time.deltaTime));
            yield return null;
        }
        
        yield return null;
    }
}
