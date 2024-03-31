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
        
        private bool isOnCooldown = false; 

        void Start()
        {
            var grandParent = transform.parent.parent;
            abilityCastHandler = grandParent.GetComponent<AbilityCastHandler>();
            playerStatsController = grandParent.GetComponent<PlayerStatsController>();
            abilityStats = GetComponent<AbilityStats>();

            abilityCastHandler.OnAbilityCast += OnAbilityUsed;
            
        }

        private void OnAbilityUsed()
        {
            StartCoroutine(RiftCoroutine());
            
            // DEN HER MÅDE AT STARTE COOLDOWN PÅ SKAL HELST BRUGES I DE ANDRE ABILITY SCRIPTS OGSÅ 
            abilityCastHandler.StartCooldown(defaultCooldown,abilityStats.GetAttackCooldown()); //!!!!
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

            float actualDuration = Time.time - startTime;

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
                }
            }
        }
    }