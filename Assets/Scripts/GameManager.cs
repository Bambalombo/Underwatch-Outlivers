using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private ExperienceController _experienceController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerParentPrefab;
    [SerializeField] private GameObject spawnerEnemyController;
    [SerializeField] private int numberOfPlayers = 1;
    private Transform _playerParent;
    private Transform _damagePopupParent;
    private Transform _experiencePickupParent;
    private Transform _pickupParent;
    private Transform _bulletParent;
    private Transform _spawnerEnemyControllerParent;
    private Transform _enemyParent;
    private Transform _bossParent;
    
    [SerializeField] private GameObject[] players; // Array to store player references

    private MultiplayerEventSystem[] playerMultiplayerEventSystems;

    
    
    
    private bool isPaused = false;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            players = new GameObject[4]; // Initialize the array for up to 4 players
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
    

    // Public static methods to access the private fields
    public static Transform GetBulletParent() => _instance._bulletParent;
    public static Transform GetDamagePopupParent() => _instance._damagePopupParent;
    public static Transform GetExperiencePickupParent() => _instance._experiencePickupParent;
    public static Transform GetPickupParent() => _instance._pickupParent;
    public static Transform GetSpawnerEnemyControllerParent() => _instance._spawnerEnemyControllerParent;
    private static GameObject[] playerGameObjectsArray => _instance.players;
    public static int GetNumberOfPlayers() => _instance.numberOfPlayers;

    public static void SetNumberOfPlayers(int value)
    {
        _instance.numberOfPlayers = value;
        //check if the number of players would either become less than 1 or more than 4
        if (_instance.numberOfPlayers < 1)
        {
            _instance.numberOfPlayers = 1;
        }
        else if (_instance.numberOfPlayers > 4)
        {
            _instance.numberOfPlayers = 4;
        }
    }
    public static Transform GetEnemyParent() => _instance._enemyParent;
    public static Transform GetBossParent() => _instance._bossParent;
    
    public static GameObject[] GetPlayerGameObjects() => _instance.FindPlayerGameObjects();
    //public static GameObject GetNearestPlayer() => _instance.FindNearestPlayer(Vector3 currentPosition);
    
    private GameObject[] FindPlayerGameObjects()
    {
        var playerGameObjects = new GameObject[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerGameObjects[i] = players[i];
        }

        return playerGameObjects;  
    }

    public static GameObject GetNearestPlayer(Vector3 currentPosition)
    {
        GameObject nearestPlayer = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject player in playerGameObjectsArray)
        {
            if (player != null)
            {
                Vector3 directionToPlayer = player.transform.position - currentPosition;
                float dSqrToPlayer = directionToPlayer.sqrMagnitude;
                if (dSqrToPlayer < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToPlayer;
                    nearestPlayer = player;
                }
            }
        }
        return nearestPlayer;
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
        _damagePopupParent = FindOrCreateParent("DamagePopupParent", _damagePopupParent);
        _experiencePickupParent = FindOrCreateParent("ExperiencePickupParent", _experiencePickupParent);
        _pickupParent = FindOrCreateParent("PickupParent", _pickupParent);
        _bulletParent = FindOrCreateParent("BulletParent", _bulletParent);

        // Only instantiate spawnerEnemyController if it has not been already
        if (_spawnerEnemyControllerParent == null)
        {
            var spawnerEnemyGameObject = Instantiate(spawnerEnemyController);
            _spawnerEnemyControllerParent = spawnerEnemyGameObject.transform;
        }
        
        _enemyParent = FindOrCreateParent("EnemyParent", _enemyParent);
        _bossParent = FindOrCreateParent("BossParent", _bossParent);
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
        Debug.Log($"Creating {playersToCreate} players");
        if (_playerParent == null)
        {
            _playerParent = Instantiate(playerParentPrefab, Vector3.zero, Quaternion.identity).transform;

            // Ensure arrays are initialized to the correct size before use.
            players = new GameObject[playersToCreate];
            playerMultiplayerEventSystems = new MultiplayerEventSystem[playersToCreate];

            for (int i = 0; i < playersToCreate; i++)
            {
                // Instantiate player at the calculated position and under _playerParent for organization
                Vector3 position = new Vector3(i * 2 - (playersToCreate - 1), 0, 0);
                players[i] = Instantiate(playerPrefab, position, Quaternion.identity, _playerParent);
                players[i].name = "Player_" + (i + 1);

                // Find the "InteractableCanvas" by iterating through child objects
                Canvas playerCanvas = null;
                foreach (Transform child in players[i].transform)
                {
                    if (child.name == "InteractableCanvas")
                    {
                        playerCanvas = child.GetComponent<Canvas>();
                        break; // Found the interactable canvas, no need to search further
                    }
                }

                // Find the MultiplayerEventSystem component within the instantiated player's children
                MultiplayerEventSystem playerEventSystem = players[i].GetComponentInChildren<MultiplayerEventSystem>(true);

                if (playerCanvas != null && playerEventSystem != null)
                {
                    // Set the MultiplayerEventSystem's playerRoot to the "InteractableCanvas" GameObject
                    playerEventSystem.playerRoot = playerCanvas.gameObject;
        
                    // Store the reference to the player's MultiplayerEventSystem for potential future use
                    playerMultiplayerEventSystems[i] = playerEventSystem;
                }
                else
                {
                    Debug.LogError("Failed to find 'InteractableCanvas' or MultiplayerEventSystem for player " + (i + 1));
                }
            }
            
        }
    }


    private void FindPlayerInputSystems(int playersToCreate)
    {
        playerMultiplayerEventSystems = new MultiplayerEventSystem[playersToCreate];
        for (int i = 0; i < playersToCreate; i++)
        {
            Debug.Log($"Attempting to find: Player{i+1}EventSystem");
            playerMultiplayerEventSystems[i] = GameObject.Find($"Player{i+1}EventSystem").GetComponent<MultiplayerEventSystem>();
            
        }
    }

    
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

    }

    public ExperienceController GetExperienceController()
    {
        return _experienceController;
    }


}

