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
        PickupItem item = null;

        if (other.CompareTag("PickupItem"))
        {
            item = other.gameObject.GetComponent<PickupItem>();
        }
        
        /*
        switch (other.gameObject.tag)
        {
            case "Experience":
                pickupable = other.gameObject.GetComponent<ExperienceAmount>();
                break;
            case "HealthPickup":
                pickupable = other.gameObject.GetComponent<HealthPickup>();
                break;
        }
        */

        if (item != null && !item.isBeingPickedUp)
        {
            Debug.Log(item.isBeingPickedUp);
            item.CheckIfBeingPickedUp(gameObject);
            StartCoroutine(MoveObjectToPlayer(other.gameObject, expMoveSpeed, item));
        }
        /*
        if (other.gameObject.CompareTag("Experience"))
        {
            var experiencePickup = other.gameObject.GetComponent<ExperienceAmount>();
            if (!experiencePickup.isBeingPickedUp)
            {
                experiencePickup.CheckIfBeingPickedUp(gameObject);
                StartCoroutine(MoveObjectToPlayer(other.gameObject, expMoveSpeed, experiencePickup));
            }
        }
        else if (other.gameObject.CompareTag("HealthPickup"))
        {
            var healthPickup = other.gameObject.GetComponent<HealthPickup>();
            if (!healthPickup.isBeingPickedUp)
            {
                healthPickup.isBeingPickedUp = true;
                StartCoroutine(MoveObjectToPlayer(other.gameObject, expMoveSpeed, healthPickup));
            }
        
        }
        */
    }

    private IEnumerator MoveObjectToPlayer(GameObject obj, float speed, PickupItem item)
    {
        if (obj == null)
            yield break;

        while (Vector3.Distance(obj.transform.position, transform.position) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, transform.position, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }

        switch (item.GetItemType())
        {
            case PickupItem.ItemType.Experience:
                experienceController.AddExperience((int)item.GetValue());
                break;
            case PickupItem.ItemType.Health:
                playerHealthController.PlayerHeal(item.GetValue());
                break;
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
