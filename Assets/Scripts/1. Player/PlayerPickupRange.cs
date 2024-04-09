using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerPickupRange : MonoBehaviour
{
    [SerializeField] private CircleCollider2D pickupRangeCollider;
    [SerializeField] private ExperienceController experienceController;
    [SerializeField] private PlayerStatsController playerStatsController;
    [SerializeField] private PlayerHealthController playerHealthController;

     
    
    [FormerlySerializedAs("towardsPlayerSpeed")]
    [Header("Movement Settings")]
    //[SerializeField] private float moveAwayDuration = 0.25f; // Duration to move away, adjust as needed
    //[SerializeField] private float awayDirectionSpeed = 5f; // Speed of moving away, adjust as needed
    [SerializeField] private float expMoveSpeed = 10f; // Speed of moving towards the player, adjust as needed
    
    private void Awake()
    {
        pickupRangeCollider.radius = playerStatsController.GetExperiencePickupRange();
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Experience") || other.gameObject.CompareTag("HealthPickup"))
        {
            StartCoroutine(MoveObjectToPlayer(other.gameObject, expMoveSpeed));
        }
    }

    private IEnumerator MoveObjectToPlayer(GameObject obj, float speed)
    {
        while (Vector3.Distance(obj.transform.position, transform.position) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }

        if (obj.CompareTag("Experience"))
        {
            var experiencePickup = obj.GetComponent<ExperienceAmount>();
            var expAmount = experiencePickup.GetExperienceAmount();
            experienceController.AddExperience(expAmount);
        }
        else if (obj.CompareTag("HealthPickup"))
        {
            var healthPickup = obj.GetComponent<HealthPickup>();
            var healAmount = healthPickup.GetHealthAmount();
            playerHealthController.PlayerHeal(healAmount);
        }
        Destroy(obj);
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
