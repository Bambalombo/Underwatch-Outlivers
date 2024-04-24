using UnityEngine;

public class FrenziedMutation : MonoBehaviour
{
    [SerializeField] private GameObject frenzyEffectPrefab;
    [SerializeField] private float defaultCooldown;
    private AbilityCastHandler abilityCastHandler;
    private AbilityStats _abilityStats;
    private GameObject _playerGameObject;
    
    [SerializeField]private AudioSource audioSource;

    public bool frenzyFiestaEnabled;

    void Start()
    {
        _playerGameObject = transform.parent.parent.gameObject;
        abilityCastHandler = _playerGameObject.GetComponent<AbilityCastHandler>();
        _abilityStats = GetComponent<AbilityStats>();
        abilityCastHandler.OnAbilityCast += OnAbilityUsed;
    }

    private void OnAbilityUsed()
    {
        if (frenzyFiestaEnabled)
        {
            // Assuming GameManager.GetPlayerGameObjects() returns all player GameObjects in the scene.
            GameObject[] playerGameObjects = GameManager.GetPlayerGameObjects();
            foreach (GameObject player in playerGameObjects)
            {
                
                if (player != _playerGameObject && player.activeSelf)  // Apply the effect to all players except the one casting it
                {
                    if (player.GetComponentInChildren<FrenziedEffect>() == null) //We check if they already have a buff enabled before applying a new one
                    {
                        ApplyFrenzyEffect(player, 0f, 1.2f); //Allied players get no health drain but also a weaker buff
                    }
                }
            }
            
        }
        
        // Frenzy effect applied to the casting player
        
        if (_playerGameObject.GetComponentInChildren<FrenziedEffect>()) // We check here if a buff is already applied (In case of another mutated berserker having cast it on the player, and if yes, we destroy that)
        {
            Destroy(_playerGameObject.GetComponentInChildren<FrenziedEffect>().gameObject);
        }
        ApplyFrenzyEffect(_playerGameObject, 2.5f, 1.5f); // The player casting it gets health drain and a stronger buff
        audioSource.Play();
        abilityCastHandler.StartCooldown(defaultCooldown, _abilityStats.GetAttackCooldown());
    }

    private void ApplyFrenzyEffect(GameObject playerGameObject, float healthDrain, float buffMultiplier)
    {
        GameObject frenzyEffectInstance = Instantiate(frenzyEffectPrefab, playerGameObject.transform.position, Quaternion.identity, playerGameObject.transform);
        frenzyEffectInstance.GetComponent<FrenziedEffect>().Initialize(playerGameObject, healthDrain, buffMultiplier, _abilityStats.GetAttackLifetime());
    }
}