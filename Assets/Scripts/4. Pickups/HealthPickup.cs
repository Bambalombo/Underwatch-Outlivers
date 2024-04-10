using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] private float healthAmount = 10; // Set the amount of health to give
    public bool isBeingPickedUp { get; set; }

    
    public float GetHealthAmount()
    {
        return healthAmount;
    }
}
