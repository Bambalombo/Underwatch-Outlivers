using UnityEngine;

public class NearestEnemyFinder : MonoBehaviour
{
    private SpawnerEnemyController _spawnerEnemyController;
    //[SerializeField] private List<GameObject> enemies;

    //[SerializeField] private GameObject nearestEnemy;
    
    private void Awake()
    {
        _spawnerEnemyController = GetComponent<SpawnerEnemyController>();
    }

    /*private void FixedUpdate()
    {
        Dictionary<GameObject, GameObject> nearestEnemies = GetNearestEnemies();
    }*/

    /*private Dictionary<GameObject, GameObject> GetNearestEnemies()
    {
        Dictionary<GameObject, GameObject> nearestEnemies = new Dictionary<GameObject, GameObject>();
        GameObject[] players = GameManager.GetPlayerGameObjects();

        foreach (var player in players)
        {
            GameObject nearestEnemy = GetNearestEnemy(player.transform.position);
            nearestEnemies.Add(player, nearestEnemy);
        }

        return nearestEnemies;
    }*/

    public GameObject GetNearestEnemy(Vector3 position)
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (var enemy in _spawnerEnemyController.GetAllEnemiesList())
        {
            if (enemy == null) continue; // Skip if the enemy has been destroyed

            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}