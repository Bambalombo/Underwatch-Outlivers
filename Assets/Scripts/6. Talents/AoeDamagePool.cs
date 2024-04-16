using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeDamagePool : MonoBehaviour
{
    public GameObject circlePrefab; // Reference to the circle sprite prefab
    public float initialSize;
    public float maxSize;
    public float growSpeed;
    public float duration;
    public float spawnChance = 50.0f; // Chance to spawn a circle

    private List<GameObject> activeCircles = new List<GameObject>();
    private List<float> startTimes = new List<float>();
    private List<float> currentSizes = new List<float>();

    // Call this to attempt to create the circle from a prefab
    public void AttemptInitialize(float damage, Vector3 position)
    {
        if (Random.value * 100 < spawnChance)
        {
            Initialize(damage, position);
        }
    }

    private void Initialize(float damage, Vector3 position)
    {
        GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
        circle.transform.localScale = new Vector3(initialSize, initialSize, initialSize); // Ensure uniform scaling

        CircleCollider2D collider = circle.GetComponent<CircleCollider2D>();
        if (collider == null)
        {
            collider = circle.AddComponent<CircleCollider2D>();
        }
        collider.isTrigger = true;

        activeCircles.Add(circle);
        startTimes.Add(Time.time);
        currentSizes.Add(initialSize);

        // Damage handler setup
        AoeDamageHandler handler = circle.AddComponent<AoeDamageHandler>(); // Ensure this script exists and is set up correctly
        handler.SetDamage(damage);
    }

    void Update()
    {
        for (int i = activeCircles.Count - 1; i >= 0; i--)
        {
            float timeActive = Time.time - startTimes[i];

            if (timeActive > duration)
            {
                Destroy(activeCircles[i]);
                activeCircles.RemoveAt(i);
                startTimes.RemoveAt(i);
                currentSizes.RemoveAt(i);
            }
            else
            {
                currentSizes[i] = Mathf.Min(maxSize, initialSize + growSpeed * timeActive);
                activeCircles[i].transform.localScale = new Vector3(currentSizes[i], currentSizes[i], currentSizes[i]);
            }
        }
    }
}

public class AoeDamageHandler : MonoBehaviour
{
    private float damage;
    private Dictionary<GameObject, float> lastDamageTime = new Dictionary<GameObject, float>();
    private float damageInterval = 1.0f; // Time in seconds between damage applications

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!lastDamageTime.ContainsKey(other.gameObject))
            {
                // If the enemy has not been damaged before, damage them immediately and record the time
                DamageEnemy(other);
                lastDamageTime[other.gameObject] = Time.time;
            }
            else
            {
                // Check if enough time has passed since the last damage
                if (Time.time - lastDamageTime[other.gameObject] >= damageInterval)
                {
                    DamageEnemy(other);
                    lastDamageTime[other.gameObject] = Time.time; // Update the last damage time
                }
            }
        }
    }

    private void DamageEnemy(Collider2D enemy)
    {
        enemy.GetComponent<EnemyCombatController>().EnemyTakeDamage(damage);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Remove the enemy from the dictionary when they leave the AoE
            if (lastDamageTime.ContainsKey(other.gameObject))
            {
                lastDamageTime.Remove(other.gameObject);
            }
        }
    }
}
