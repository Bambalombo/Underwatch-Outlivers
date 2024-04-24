using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private ExperienceController _experienceController;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerParentPrefab;
    [SerializeField] private GameObject spawnerEnemyController;
    [SerializeField] private int numberOfPlayers = 1;
    [FormerlySerializedAs("endGameWhenPlayersDie")] [SerializeField] private bool gameOverEnabled = true;
    private Transform _playerParent;
    private Transform _damagePopupParent;
    private Transform _experiencePickupParent;
    private Transform _pickupParent;
    private Transform _bulletParent;
    private Transform _spawnerEnemyControllerParent;
    private Transform _enemyParent;
    private Transform _bossParent;

    private static List<int> _selectedCharacters;
    
    
    [SerializeField] private GameObject[] players; // Array to store player references

    private MultiplayerEventSystem[] playerMultiplayerEventSystems;
    private PlayerHealthController[] _playerHealthControllers;
    private PlayerStatsController[] _playerStatsControllers;

    private static bool _isPaused;
    private bool _isGameOver;
    
    //UI 
    public GameObject DefaultMenuUI;
    public GameObject CharacterSelectionUI;
    private FixedMultiplayerCamera _cameraManager;
    private DeathIconTimer _deathIconTimer;


    private void Awake()
    {
        _isPaused = false;
        
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

    private void Start()
    {
        UpdateCameraReference();
    }

    private void UpdateCameraReference()
    {
        _cameraManager = Camera.main.GetComponent<FixedMultiplayerCamera>();
    }
    
    // Public static methods to access the private fields
    public static Transform GetBulletParent() => _instance._bulletParent;
    public static Transform GetDamagePopupParent() => _instance._damagePopupParent;
    public static Transform GetExperiencePickupParent() => _instance._experiencePickupParent;
    public static Transform GetPickupParent() => _instance._pickupParent;
    public static Transform GetSpawnerEnemyControllerParent() => _instance._spawnerEnemyControllerParent;
    private static GameObject[] PlayerGameObjectsArray => _instance.players;
    public static int GetNumberOfPlayers() => _instance.numberOfPlayers;
    
    public static PlayerHealthController[] GetPlayerHealthControllers() => _instance._playerHealthControllers;

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

        for (int i = 0; i < GetNumberOfPlayers(); i++)
        {
            Vector3 directionToPlayer = PlayerGameObjectsArray[i].transform.position - currentPosition;
            
            float dSqrToPlayer = directionToPlayer.sqrMagnitude;
            if (dSqrToPlayer < closestDistanceSqr && GetPlayerHealthControllers()[i].IsAlive())
            {
                closestDistanceSqr = dSqrToPlayer;
                nearestPlayer = PlayerGameObjectsArray[i];
            }
        }
        
        /*
        foreach (GameObject player in PlayerGameObjectsArray)
        {
            if (player != null)
            {
                Vector3 directionToPlayer = player.transform.position - currentPosition;
                float dSqrToPlayer = directionToPlayer.sqrMagnitude;
                if (dSqrToPlayer < closestDistanceSqr && player.GetComponent<PlayerHealthController>().CheckIfAlive()
                {
                    closestDistanceSqr = dSqrToPlayer;
                    nearestPlayer = player;
                }
            }
        }
        */
        return nearestPlayer;
    }

    public bool EndGameEnabled()
    {
        return gameOverEnabled;
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnPlayerDeath += KillPlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnPlayerDeath -= KillPlayer;
    }
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void EnableCharacterSelectionUI()
    {
        DefaultMenuUI.SetActive(false);
        CharacterSelectionUI.SetActive(true);
        CharacterSelectionUI.GetComponent<CharacterSelectionManager>().EnableCharacterSelectionUI(GetNumberOfPlayers());
    }

    public static void SaveCharacterSelectionsAndLoadLevel(List<int> listOfSelectedCharacters)
    {
        _selectedCharacters = listOfSelectedCharacters;
        SceneManager.LoadScene("Level_1");
    }

    public static event Action OnGameOver;

    public void StartGameOverSequence()
    {
        Debug.Log("Running GameOver Sequence.");
        _isGameOver = true;
        OnGameOver?.Invoke();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level_1")
        {
            SetupSceneDependencies();
            CreatePlayers(numberOfPlayers);
        }
        
        UpdateCameraReference();
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
        //Debug.Log($"Creating {playersToCreate} players");
        if (_playerParent == null)
        {
            _playerParent = Instantiate(playerParentPrefab, Vector3.zero, Quaternion.identity).transform;

            // Ensure arrays are initialized to the correct size before use.
            players = new GameObject[playersToCreate];
            playerMultiplayerEventSystems = new MultiplayerEventSystem[playersToCreate];
            _playerHealthControllers = new PlayerHealthController[playersToCreate];
            _playerStatsControllers = new PlayerStatsController[playersToCreate];

            for (int i = 0; i < playersToCreate; i++)
            {
                // Instantiate player at the calculated position and under _playerParent for organization
                Vector3 position = new Vector3(i * 2 - (playersToCreate - 1), 0, 0);
                players[i] = Instantiate(playerPrefab, position, Quaternion.identity, _playerParent);
                players[i].name = "Player_" + (i + 1);
                
                // Fill local lists of health and stats controllers
                PlayerHealthController playerHealthController = players[i].GetComponentInChildren<PlayerHealthController>(true);
                _playerHealthControllers[i] = playerHealthController;
                PlayerStatsController playerStatsController = players[i].GetComponentInChildren<PlayerStatsController>(true);
                playerStatsController.SetPlayerIndex(i);
                _playerStatsControllers[i] = playerStatsController;
                
                //Set the class of the player
                players[i].GetComponent<ClassAssets>().ChangeClass(_selectedCharacters[i]);

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
            
            _deathIconTimer = GameObject.FindWithTag("DeathIcons").GetComponent<DeathIconTimer>();
            _deathIconTimer.CreateDeathIcons(playersToCreate, _selectedCharacters);
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
        // Debug.Log($"Timescale is{Time.timeScale}");
        if (_isPaused) //If game is paused, unpause it and set the _isPaused to false
        {
            //Debug.Log("I am unpausing");
            _isPaused = false;
            Time.timeScale = 1;
        }

        else if (_isPaused == false)
        {
            //Debug.Log("I am pausing");
            _isPaused = true;
            Time.timeScale = 0;
        }
    }

    public ExperienceController GetExperienceController()
    {
        return _experienceController;
    }

    public delegate void PlayerDeathHandler(int playerIndex, float respawnTime);
    public static event PlayerDeathHandler OnPlayerDeath;
    public void PlayerDied(int playerIndex, float respawnTime)
    {
        OnPlayerDeath?.Invoke(playerIndex, respawnTime);
        StartCoroutine(KillAndRespawnPlayer(playerIndex, respawnTime));
        
        // Check to see if we end the game
        bool anyPlayersAreAlive = GetPlayerHealthControllers().Any(player => player.IsAlive());
        if (!anyPlayersAreAlive && EndGameEnabled())
            StartGameOverSequence();
    }

    private void KillPlayer(int playerIndex, float respawnTime)
    {
        StartCoroutine(KillAndRespawnPlayer(playerIndex, respawnTime));
    }

    private IEnumerator KillAndRespawnPlayer(int playerIndex, float respawnTime)
    {
        var player = players[playerIndex];
        player.SetActive(false);
        
        //Debug.Log("Respawn time " + respawnTime);
        
        yield return new WaitForSeconds(respawnTime);

        if (_isGameOver)
            yield break;
        
        player.SetActive(true);
        
        var newPos = _cameraManager.GetCenterPoint();
        player.transform.position = newPos;

        foreach (var pos in _playerStatsControllers)
        {
            Debug.Log($"new pos, {pos.GetPlayerPosition()}");
        }
        
        PlayerRespawned(playerIndex);
    }
    
    public List<int> GetAlivePlayers()
    {
        List<int> alivePlayers = new List<int>();
        for (int i = 0; i < _playerHealthControllers.Length; i++)
        {
            if (_playerHealthControllers[i].IsAlive())
            {
                alivePlayers.Add(i);
            }
        }
        return alivePlayers;
    }
    
    public delegate void PlayerRespawnHandler(int playerIndex);

    public static event PlayerRespawnHandler OnPlayerRespawn;

    public void PlayerRespawned(int playerIndex)
    {
        OnPlayerRespawn?.Invoke(playerIndex);
    }
    
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
    
    public static void DestroyInstance()
    {
        Destroy(_instance.gameObject);
    }
    
}

