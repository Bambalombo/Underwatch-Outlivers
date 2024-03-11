using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject spawnerEnemyController;
    [SerializeField] private int numberOfPlayers = 1;
    private Transform _playerParent;
    private Transform _damagePopupParent;
    private Transform _experiencePickupParent;
    private Transform _pickupParent;
    private Transform _bulletParent;
    
    private GameObject[] _players; // Array to store player references
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _players = new GameObject[4]; // Initialize the array for up to 4 players
            SceneManager.sceneLoaded += OnSceneLoaded;

            if (SceneManager.GetActiveScene().name == "Level_1")
            {
                SetupSceneDependencies();
                CreatePlayers(numberOfPlayers);
            }
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    

    // Public static methods to access the parents
    public static Transform GetBulletParent() => _instance._bulletParent;
    public static Transform GetDamagePopupParent() => _instance._damagePopupParent;
    public static Transform GetExperiencePickupParent() => _instance._experiencePickupParent;
    public static Transform GetPickupParent() => _instance._pickupParent;
    public static GameObject GetPlayer(int index)
    {
        if (index >= 0 && index < _instance._players.Length)
        {
            return _instance._players[index];
        }
        return null;
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // Load level 1 with menu button
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level_1");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level_1")
        {
            SetupSceneDependencies();
            CreatePlayers(numberOfPlayers);
        }
    }
    
    private void SetupSceneDependencies()
    {
        _playerParent = FindOrCreateParent("PlayerParent", _playerParent);
        _damagePopupParent = FindOrCreateParent("DamagePopupParent", _damagePopupParent);
        _experiencePickupParent = FindOrCreateParent("ExperiencePickupParent", _experiencePickupParent);
        _pickupParent = FindOrCreateParent("PickupParent", _pickupParent);
        _bulletParent = FindOrCreateParent("BulletParent", _bulletParent);
        
        Instantiate(spawnerEnemyController);
    }

    private static Transform FindOrCreateParent(string parentName, Transform parentTransform)
    {
        // Return the existing transform if it's already assigned
        if (parentTransform != null) 
            return parentTransform; 
    
        // Otherwise, find or create the object
        var parentObj = GameObject.Find(parentName);
        if (parentObj == null)
            parentObj = new GameObject(parentName);
        
        return parentObj.transform;
    }

    
    private void CreatePlayers(int playersToCreate)
    {
        // Check if playerParent exists or create it
        _playerParent = FindOrCreateParent("PlayerParent", _playerParent);
        
        for (int i = 0; i < playersToCreate; i++)
        {
            // Calculate the position for each player
            Vector3 position = new Vector3(i * 2 - (playersToCreate - 1), 0, 0);
            // Instantiate and store the player reference in the array
            _players[i] = Instantiate(playerPrefab, position, Quaternion.identity, _playerParent);
            _players[i].name = "Player_" + (i + 1); // Naming the player GameObject
        }
    }
}

