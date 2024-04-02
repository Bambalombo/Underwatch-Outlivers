using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerParentPrefab;
    [SerializeField] private GameObject spawnerEnemyController;
    [SerializeField] private ExperienceController _experienceController;
    [SerializeField] private int numberOfPlayers = 1;
    private Transform _playerParent;
    private Transform _damagePopupParent;
    private Transform _experiencePickupParent;
    private Transform _pickupParent;
    private Transform _bulletParent;
    private Transform _spawnerEnemyControllerParent;
    
    [SerializeField] private GameObject[] players; // Array to store player references
    
    private MultiplayerEventSystem[] playerInputSystems;

    
    
    
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
        else
        {
            // Reset or adjust existing _spawnerEnemyControllerParent as needed
        }
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
        if (_playerParent == null)
        {
            _playerParent = Instantiate(playerParentPrefab, Vector3.zero, Quaternion.identity).transform;

            for (int i = 0; i < playersToCreate; i++)
            {
                Vector3 position = new Vector3(i * 2 - (playersToCreate - 1), 0, 0);
                players[i] = Instantiate(playerPrefab, position, Quaternion.identity, _playerParent);
                players[i].name = "Player_" + (i + 1);
            }
            
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

