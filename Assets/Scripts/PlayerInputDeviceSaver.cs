using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDeviceSaver : MonoBehaviour
{
    //Det her script fixer forhåbentligt at controllers bliver switched mellem characters ved respawn
    //Idéen er at den gemmer det input device som spilleren bliver assigned med det samme vi spawner dem ind i level 1, og så reassigner deviced hver gang player gameobject bliver enabled igen (og dermed overrider wtf det end er unity input system har gang i)
    private PlayerInput playerInput;
    private InputDevice initialDevice;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        
        if (playerInput != null)
        {

            try //Har bare lavet en try catch her fordi hvis jeg tjekker om playerInput.devices[0] == null crasher den og ved ikk hvordan man ellers kan tjekke om det er et device connected
            {
                Debug.Log($"Assigning {playerInput.devices[0]} to : {gameObject.name}");
                initialDevice = playerInput.devices[0];
            }
            catch
            {
                Debug.Log("Failed to save controllers probably due to not enough input devices connected");
            }
            
        }
    }

    void OnEnable()
    {
        Debug.Log($"JEG KØRERERER FRA {gameObject.name}");
        if (playerInput != null && initialDevice != null)
        {
            playerInput.SwitchCurrentControlScheme(initialDevice);
        }
    }
}