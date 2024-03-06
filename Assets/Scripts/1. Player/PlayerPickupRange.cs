using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerPickupRange : MonoBehaviour
{
    private CircleCollider2D _pickupRangeCollider;
    [SerializeField] private float pickupRange = 4f;
    [SerializeField] private PlayerExperience playerExperience;
    
    void Start()
    {
        _pickupRangeCollider = gameObject.GetComponent<CircleCollider2D>();    
        _pickupRangeCollider.radius = pickupRange;
    }

    private void ExperiencePickupAnimation()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Experience"))
        {
            StartCoroutine(MoveExperienceToPlayer(other.gameObject));
        }
    }

    private IEnumerator MoveExperienceToPlayer(GameObject expObject)
    {
        var experiencePickup = expObject.GetComponent<ExperienceAmount>();
        var expAmount = experiencePickup.GetExperienceAmount();
    
        while (expObject != null && Vector3.Distance(expObject.transform.position, transform.position) > 0.1f)
        {
            expObject.transform.position = Vector3.MoveTowards(expObject.transform.position, transform.position, Time.deltaTime * 10); // Adjust speed as needed
            yield return null;
        }

        if (expObject != null)
        {
            playerExperience.GainExperience(expAmount);
            Destroy(expObject);
        }
    }

}
