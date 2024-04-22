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
                if (player != _playerGameObject)  // Apply the effect to all players except the one casting it
                {
                    ApplyFrenzyEffect(player, 0f, 1.2f); //Allied players get no health drain but also a weaker buff
                    
                }
            }
            
        }
        ApplyFrenzyEffect(_playerGameObject, 0.1f, 2f); //The player casting it gets health drain and a stronger buff
        audioSource.Play();
        abilityCastHandler.StartCooldown(defaultCooldown, _abilityStats.GetAttackCooldown());
    }

    private void ApplyFrenzyEffect(GameObject playerGameObject, float healthDrain, float buffMultiplier)
    {
        GameObject frenzyEffectInstance = Instantiate(frenzyEffectPrefab, playerGameObject.transform.position, Quaternion.identity, playerGameObject.transform);
        frenzyEffectInstance.GetComponent<FrenziedEffect>().Initialize(playerGameObject, healthDrain, buffMultiplier);
    }
}