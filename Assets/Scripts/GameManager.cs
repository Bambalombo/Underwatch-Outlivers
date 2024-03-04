using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject player; 
    private Transform _playerParent;
    [SerializeField] private int numberOfPlayers = 1;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Check if the current scene is Level_1 and instantiate the player if so
            if (SceneManager.GetActiveScene().name == "Level_1")
            {
                CreatePlayers(numberOfPlayers);
            }
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
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
            CreatePlayers(numberOfPlayers);
        }
    }

    private void CreatePlayers(int players)
    {
        // Check if playerParent exists or create it
        if (_playerParent == null)
        {
            _playerParent = new GameObject("PlayerParent").transform;
        }

        switch (players)
        {
            case 1:
                Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity, _playerParent);
                break;
            case 2:
                Instantiate(player, new Vector3(-2, 0, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(2, 0, 0), Quaternion.identity, _playerParent);
                break;
            case 3:
                Instantiate(player, new Vector3(-2, 0, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(2, 0, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity, _playerParent);
                break;
            case 4:
                Instantiate(player, new Vector3(-2, 0, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(2, 0, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(0, 2, 0), Quaternion.identity, _playerParent);
                Instantiate(player, new Vector3(0, -2, 0), Quaternion.identity, _playerParent);
                break;
            default:
                Debug.Log("Unsupported number of players: " + players);
                Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity, _playerParent);
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

