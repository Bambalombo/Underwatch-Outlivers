using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public class AbilityCastHandler : MonoBehaviour
{
    //Kan ikk rigtigt lide det her script men ved ikk hvordan man ellers skal gøre det eftersom vi har dynamically assigned abilities
    // Start is called before the first frame update
    
    // Define a public event that other scripts can subscribe to
    public event Action OnAbilityCast;

    // This method can be called to trigger the event
    public void CastAbility()
    {
        // Check if there are any subscribers to the event
        if (OnAbilityCast != null)
        {
            // Invoke the event
            OnAbilityCast();
        }
    }
    
}
