using UnityEngine;

public class ExperienceAmount : MonoBehaviour
{
    [SerializeField] private int experienceAmount = 10; // Set the amount of EXP to give
    //[SerializeField] private IntVariable playerExperience;
    
    // Get experienceAmount from the enemy that dropped the EXP
    public int GetExperienceAmount()
    {
        return experienceAmount;
    }
    
    /*public void GainExperience()
    {
        playerExperience.value += experienceAmount;
        Destroy(gameObject);
    }*/
    
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure to set the player's tag to "Player"
        {
            Debug.Log("Player collected EXP");
            PlayerExperience playerExperience = other.GetComponent<PlayerExperience>();
            if (playerExperience != null)
            {
                playerExperience.GainExperience(experienceAmount);
                Destroy(gameObject); // Destroy the EXP pickup after it's collected
            }
        }
    }*/
}