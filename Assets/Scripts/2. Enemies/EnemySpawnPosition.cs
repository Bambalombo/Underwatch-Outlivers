using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPosition : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 40;
    [SerializeField] private float safeZoneRadius = 30;
 
    [SerializeField] private List<Vector2> bossSpawnPositions;

    
    public Vector2 CalculateSpawnPosition(Vector2 spawnFromPosition)
    {
        Vector2 spawnPos;
        int maxAttempts = 10;
        int attempts = 0;

        do
        {
            // Generate a random angle
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

            // Random distance from player within spawn radius, but outside the safe zone
            float distance = Random.Range(safeZoneRadius, spawnRadius);

            // Calculate spawn position using angle and distance
            spawnPos = new Vector2(
                spawnFromPosition.x + Mathf.Cos(angle) * distance,
                spawnFromPosition.y + Mathf.Sin(angle) * distance
            );

            attempts++;
        } while (!IsValidSpawnPosition(spawnPos) && attempts < maxAttempts);

        // If a valid position isn't found after maxAttempts, fallback to a default
        if (attempts >= maxAttempts)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            spawnPos = new Vector2(
                spawnFromPosition.x + Mathf.Cos(angle) * safeZoneRadius,
                spawnFromPosition.y + Mathf.Sin(angle) * safeZoneRadius
            );
        }

        return spawnPos;
    }
    
    private bool IsValidSpawnPosition(Vector2 position)
    {
        // Check if the position is within the spawn radius and outside the safe zone radius
        /*bool isWithinSpawnRadius = Physics2D.OverlapCircle(position, spawnRadius) != null;
        bool isOutsideSafeZoneRadius = Physics2D.OverlapCircle(position, safeZoneRadius) == null;

        return isWithinSpawnRadius && isOutsideSafeZoneRadius;*/

        return true; // Temporarily returning true
    }
    
    
    public Vector2 RandomBossSpawnPosition()
    {
        int randomIndex = Random.Range(0, bossSpawnPositions.Count);
        Vector2 spawnPos = bossSpawnPositions[randomIndex];
        
        bossSpawnPositions.Remove(spawnPos);
        
        return spawnPos;
    }
    
}
