using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

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
    
    [SerializeField] private GameObject[] players; // Array to store player references


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
        //_playerParent = FindOrCreateParent("PlayerParent", _playerParent);
        _damagePopupParent = FindOrCreateParent("DamagePopupParent", _damagePopupParent);
        _experiencePickupParent = FindOrCreateParent("ExperiencePickupParent", _experiencePickupParent);
        _pickupParent = FindOrCreateParent("PickupParent", _pickupParent);
        _bulletParent = FindOrCreateParent("BulletParent", _bulletParent);
        
        var spawnerEnemyGameObject = Instantiate(spawnerEnemyController);
        _spawnerEnemyControllerParent = spawnerEnemyGameObject.transform;
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
    
    //Den her funktion skal ændres eventually så den kun bliver called 1 gang (Lige nu bliver den called 2 gange hvis men starter spillet fra menu)
    private void CreatePlayers(int playersToCreate)
    {
        // Check if playerParent exists or create it
        // (Har ændret den sådan den ikk caller findOrCreateParent, men bare instantiater direkte fordi jeg skulle bruge et anderledes parent object)
        _playerParent = Instantiate(playerParentPrefab, Vector3.zero, Quaternion.identity).transform;
        
        for (int i = 0; i < playersToCreate; i++)
        {
            // Calculate the position for each player
            Vector3 position = new Vector3(i * 2 - (playersToCreate - 1), 0, 0);
            // Instantiate and store the player reference in the array
            players[i] = Instantiate(playerPrefab, position, Quaternion.identity, _playerParent);
            players[i].name = "Player_" + (i + 1); // Naming the player GameObject
            // Assign the control scheme based on player index
            var playerInput = players[i].GetComponent<PlayerInput>();
            

        }
    }
}   

