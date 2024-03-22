using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AbilityCastHandler : MonoBehaviour
{
    public event Action OnAbilityCast;
    public Slider cooldownSlider; 
    private bool isOnCooldown = false;


    // This method can be called to trigger the event
    public void CastAbility()
    {
        // Check if there are any subscribers to the event
        if (OnAbilityCast != null && !isOnCooldown)
        {
            // Invoke the event
            OnAbilityCast();
            isOnCooldown = true;
        }
    }
    
    public void StartCooldown(float defaultCooldown, float abilityStatCooldown)
    {
        StartCoroutine(AbilityCooldown(defaultCooldown - abilityStatCooldown));
    }

    private IEnumerator AbilityCooldown(float cooldownTime)
    {
        isOnCooldown = true;
        float elapsedTime = 0f;

        // Initialize your UI cooldown bar here, setting its value to 0
        UpdateCooldownUI(0);

        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / cooldownTime;
            UpdateCooldownUI(progress); // Update the UI based on the elapsed time
            yield return null; // Wait until next frame
        }

        UpdateCooldownUI(1); // Ensure the UI shows the ability is ready (full bar)
        isOnCooldown = false; // Reset cooldown status
    }

    void UpdateCooldownUI(float progress)
    {
        if (cooldownSlider != null)
        {
            cooldownSlider.value = progress;
        }
    }
    
}
