using System.Collections;
using UnityEngine;

public class AbyssalRift : MonoBehaviour
{

    public float riftSpawnDistance; // Distance in front of the player where the rift spawns
    public float riftTravelDistance; // Max distance the rift should travel from its spawn point
    public float riftTravelSpeed; // Speed at which the rift travels
    public float riftInitialScale;// Initial scale of the rift
    public float riftMaxScale; // Maximum scale the rift will reach
    [SerializeField] private float defaultCooldown;
    public GameObject riftPrefab;
    [SerializeField] private float pullStrength;
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats;
    private WeaponStats _weaponStats;
    private PlayerStatsController playerStatsController;
    private PlayerHealthController _playerHealthController;
    private NearestEnemyFinder _nearestEnemyFinder;
    
    
    
    [SerializeField] private AudioClip[] arraySounds;
    private int arrayMax;
    private int soundToPlay;
    [SerializeField] AudioSource audioSource;
    
    private bool isOnCooldown = false;
    
    //Soul harvest talent variables
    public bool soulHarvestTalentActivated;
    public float soulHarvestHealAmount;
    public float soulHarvestHealChance; // Chance to trigger the soul harvest heal
    public float nextHealTime; // Next time player can be healed by soul harvest
    public bool deadlyLitterActivated;
    public GameObject deadlyLitterMinionPrefab;

    void Start()
    {
        var grandParent = transform.parent.parent;
        _weaponStats = grandParent.GetComponentInChildren<WeaponStats>();
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        _playerHealthController = grandParent.GetComponent<PlayerHealthController>();
        _nearestEnemyFinder = GameManager.GetSpawnerEnemyControllerParent().GetComponent<NearestEnemyFinder>();
        abilityStats = GetComponent<AbilityStats>();

        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
    }

    private void OnAbilityUsed()
    {
        StartCoroutine(RiftCoroutine());
        abilityCastHandler.StartCooldown(defaultCooldown, abilityStats.GetAttackCooldown());
        arrayMax = arraySounds.Length;
        soundToPlay = Random.Range(0, arrayMax);
        audioSource.clip = arraySounds[soundToPlay];
        audioSource.Play();
    }

    IEnumerator RiftCoroutine()
    {
        Vector2 spawnDirection = playerStatsController.GetLastMoveDirection().normalized;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * riftSpawnDistance;
        GameObject rift = Instantiate(riftPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 1), Quaternion.identity);
        rift.transform.localScale = new Vector3(riftInitialScale, riftInitialScale, 0);
        
        if (deadlyLitterActivated)
        {
            StartCoroutine(DeadlyLitterCoroutine(rift.transform));
        }


        Vector2 targetPosition = spawnPosition + spawnDirection * riftTravelDistance;
        float startTime = Time.time;
        float endTime = startTime + abilityStats.GetAttackLifetime();
        float journeyLength = Vector2.Distance(spawnPosition, targetPosition);
        float fracJourney = 0f;

        while (Time.time < endTime)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }

            // Update position of rift
            float distCovered = (Time.time - startTime) * riftTravelSpeed;
            fracJourney = distCovered / journeyLength;
            rift.transform.position = Vector3.Lerp(spawnPosition, targetPosition, fracJourney);

            // Scale the rift
            float scale = Mathf.Lerp(riftInitialScale, riftMaxScale, fracJourney);
            rift.transform.localScale = new Vector3(scale, scale, 0);

            // Example of pulling enemies towards the center
            PullEnemiesTowardsCenter(rift.transform.position, scale / 2f);
            yield return null;
        }

        if (deadlyLitterActivated)
        {
            StopCoroutine(DeadlyLitterCoroutine());
        }
        Destroy(rift);
    }

    void PullEnemiesTowardsCenter(Vector2 riftPosition, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(riftPosition, radius);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Rigidbody2D enemyRb = hitCollider.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    Vector2 directionToCenter = riftPosition - (Vector2)hitCollider.transform.position;
                    enemyRb.AddForce(directionToCenter.normalized * pullStrength, ForceMode2D.Force);
                }
                //Soul Harvest talent
                if (soulHarvestTalentActivated && Time.time >= nextHealTime)
                {
                    nextHealTime = Time.time + 0.5f; // Set the next allowable heal time
                    //Debug.Log("Checking random value");
                    if (Random.value < soulHarvestHealChance)
                    {
                        _playerHealthController.PlayerHeal(soulHarvestHealAmount);
                    }
                }
            }
            if (hitCollider.CompareTag("Player"))
            {
                Rigidbody2D playerRb = hitCollider.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 directionToCenter = riftPosition - (Vector2)hitCollider.transform.position;
                    playerRb.AddForce(directionToCenter.normalized * pullStrength, ForceMode2D.Force);
                }
            }
        }

    }

    IEnumerator DeadlyLitterCoroutine(Transform spawnPosition = null)
    {
        while (true)
        {
            SpawnMinion(spawnPosition);
            yield return new WaitForSeconds(3f);
        }
    }

    private void SpawnMinion(Transform spawnPosition)
    {
        if (spawnPosition)
        {
            GameObject minion = Instantiate(deadlyLitterMinionPrefab, spawnPosition.position, Quaternion.identity);
            minion.GetComponent<DeadlyLitterPrefabScript>().Initialize(abilityStats, _nearestEnemyFinder,_weaponStats);
        }
    }
}
