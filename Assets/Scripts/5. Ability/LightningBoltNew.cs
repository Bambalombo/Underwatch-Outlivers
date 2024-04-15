using UnityEngine;

public class LightningBoltNew : MonoBehaviour
{
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private GameObject lightningBoltPrefab;

    [SerializeField] private float boltStartSize = 0.8f;
    [SerializeField] private float boltMaxSize = 2.8f;
    [SerializeField] private float lightningWidth = 1f;

    [SerializeField] private float defaultCooldown;
    private AbilityCastHandler abilityCastHandler;
    private AbilityStats abilityStats;
    private BulletController bc;
    private ParticleSystem ps;

    private void Awake()
    {
        var grandParent = transform.parent.parent;
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();

        // ps = GetComponent<ParticleSystem>();
        // ParticleSystem.MainModule main = ps.main;
        // main.simulationSpace = ParticleSystemSimulationSpace.World;

        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
        abilityStats = GetComponent<AbilityStats>();
    }

    private void OnAbilityUsed()
    {
        //Det her er lidt ødelægger pointen med at charge den, men kan ikk finde en måde at lave GetKeyUp på med nye input system, sorry Linus ;_; (Ability bliver triggered med Q btw)
        CastLightningBolt();
        abilityCastHandler.StartCooldown(defaultCooldown, abilityStats.GetAttackCooldown());
    }

    private void CastLightningBolt()
    {
        Vector2 spawnPosition = gameObject.transform.position;
        var boltInstance = Instantiate(lightningBoltPrefab, spawnPosition, Quaternion.identity);
    
        bc = boltInstance.GetComponent<BulletController>();
        boltInstance.GetComponent<LightningBoltBall>().SetAbilityStatsReference(abilityStats);
        
        var flyingBoltSize = boltStartSize + (abilityStats.GetDamage() / 100) * boltMaxSize;
        boltInstance.transform.localScale = new Vector3(flyingBoltSize, flyingBoltSize, 1);
        
        ps = boltInstance.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = ps.main;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.position = spawnPosition;
        emitParams.ResetStartSize();
        emitParams.startSize = 0.2f + (abilityStats.GetDamage() / 100) * 1;
        ps.Emit(emitParams, 1);

        Vector2 spawnDirection = playerStatsController.GetLastMoveDirection().normalized;
        bc.Initialize(spawnDirection, abilityStats.GetProjectileSpeed(), abilityStats, 3);
    }
}