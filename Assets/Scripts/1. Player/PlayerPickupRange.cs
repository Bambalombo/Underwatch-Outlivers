using System.Collections;
using UnityEngine;

public class PlayerPickupRange : MonoBehaviour
{
    private CircleCollider2D _pickupRangeCollider;
    //[SerializeField] private float pickupRange = 4f;
    [SerializeField] private ExperienceController experienceController;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private PlayerHealthController playerHealthController;
    
    [Header("Movement Settings")]
    //[SerializeField] private float moveAwayDuration = 0.25f; // Duration to move away, adjust as needed
    //[SerializeField] private float awayDirectionSpeed = 5f; // Speed of moving away, adjust as needed
    [SerializeField] private float towardsPlayerSpeed = 10f; // Speed of moving towards the player, adjust as needed
    
    private void Awake()
    {
        _pickupRangeCollider = GetComponent<CircleCollider2D>();    
        _pickupRangeCollider.radius = playerStatsController.GetExperiencePickupRange();
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Experience"))
        {
            var experiencePickup = other.gameObject.GetComponent<ExperienceAmount>();
            var expAmount = experiencePickup.GetExperienceAmount();
            StartCoroutine(MoveObjectToPlayer(other.gameObject, towardsPlayerSpeed));
            if (other.gameObject != null)
            {
                experienceController.AddExperience(expAmount);
            }
        }

        if (other.gameObject.CompareTag("HealthPickup"))
        {
            var healthPickup = other.gameObject.GetComponent<HealthPickup>();
            var healAmount = healthPickup.GetHealthAmount();
            StartCoroutine(MoveObjectToPlayer(other.gameObject, towardsPlayerSpeed));
            if (other.gameObject != null)
            {
                playerHealthController.PlayerHeal(healAmount);
            }
        }
    }

    private IEnumerator MoveObjectToPlayer(GameObject obj, float speed)
    {
        while (obj != null && Vector3.Distance(obj.transform.position, transform.position) > 0.01f)
        {
            float step = speed * Time.deltaTime;
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, 
                Time.deltaTime * towardsPlayerSpeed);
            yield return new WaitForEndOfFrame();
        }
        if (obj != null)
        {
            Destroy(obj);
        }
    }
    
    /*private IEnumerator MoveExperienceToPlayer(GameObject expObject)
    {
        var experiencePickup = expObject.GetComponent<ExperienceAmount>();
        var expAmount = experiencePickup.GetExperienceAmount();

        // Calculate the initial away direction
        Vector3 awayDirection = (expObject.transform.position - transform.position).normalized;
        float startTime = Time.time;

        // Initial movement away from the player
        while (Time.time < startTime + moveAwayDuration && expObject != null)
        {
            expObject.transform.position += awayDirection * Time.deltaTime * awayDirectionSpeed;
            yield return null;
        }

        // Move towards the player after moving away
        while (expObject != null && Vector3.Distance(expObject.transform.position, transform.position) > 0.1f)
        {
            expObject.transform.position = Vector3.MoveTowards(expObject.transform.position, transform.position, Time.deltaTime * towardsPlayerSpeed);
            yield return null;
        }

        // Once the experience reaches the player, add the experience and destroy the object
        if (expObject != null)
        {
            experienceController.GainExperience(expAmount);
            Destroy(expObject);
        }
    }*/

}
