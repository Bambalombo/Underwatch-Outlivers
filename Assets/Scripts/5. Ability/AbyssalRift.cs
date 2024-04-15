using System.Collections;
using UnityEngine;

public class AbyssalRift : MonoBehaviour
{
    [SerializeField] private float defaultCooldown;
    public GameObject riftPrefab;
    [SerializeField] private float pullStrength;
    [SerializeField] private float riftSpawnDistance;
    private AbilityCastHandler abilityCastHandler; 
    private AbilityStats abilityStats; 
    private PlayerStatsController playerStatsController;
    private PlayerHealthController _playerHealthController;
    
    private bool isOnCooldown = false;
    
    //Soul harvest talent variables
    public bool soulHarvestTalentActivated;
    public float soulHarvestHealAmount;
    public float soulHarvestHealChance; // Chance to trigger the soul harvest heal
    public float nextHealTime; // Next time player can be healed by soul harvest

    void Start()
    {
        var grandParent = transform.parent.parent;
        abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
        playerStatsController = grandParent.GetComponent<PlayerStatsController>();
        _playerHealthController = grandParent.GetComponent<PlayerHealthController>();
        abilityStats = GetComponent<AbilityStats>();

        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
    }

    private void OnAbilityUsed()
    {
        StartCoroutine(RiftCoroutine());
        abilityCastHandler.StartCooldown(defaultCooldown, abilityStats.GetAttackCooldown());
    }

    IEnumerator RiftCoroutine()
    {
        Vector2 spawnDirection = playerStatsController.GetLastMoveDirection().normalized;
        Vector2 spawnPosition = (Vector2)transform.position + spawnDirection * riftSpawnDistance;
        GameObject rift = Instantiate(riftPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 1), Quaternion.identity);

        float startTime = Time.time;
        float endTime = startTime + abilityStats.GetAttackLifetime();
        while (Time.time < endTime)
        {
            PullEnemiesTowardsCenter(rift.transform.position, rift.GetComponent<CircleCollider2D>().radius);
            yield return null;
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
}
