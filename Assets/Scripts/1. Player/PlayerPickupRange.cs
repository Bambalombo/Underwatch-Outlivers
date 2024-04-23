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

        if (item != null && !item.isBeingPickedUp)
        {
            item.CheckIfBeingPickedUp(gameObject);
            StartCoroutine(MoveObjectToPlayer(other.gameObject, expMoveSpeed, item));
        }
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
}
