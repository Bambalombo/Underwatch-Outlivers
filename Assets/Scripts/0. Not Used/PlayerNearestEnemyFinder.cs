using UnityEngine;

public class PlayerNearestEnemyFinder : MonoBehaviour
{
    public GameObject nearestEnemy;
    private readonly int checkEveryNFrames = 10; // Check every 10 frames
    private int frameCount;

    private void FixedUpdate()
    {
        frameCount++;
        if (frameCount >= checkEveryNFrames)
        {
            nearestEnemy = GetNearestEnemy();
            frameCount = 0;
        }
    }

    public GameObject GetNearestEnemy()
    {
        GameObject nearest = null;
        var closestDistanceSqr = Mathf.Infinity;
        var currentPosition = transform.position;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var directionToEnemy = enemy.transform.position - currentPosition;
            var dSqrToEnemy = directionToEnemy.sqrMagnitude;

            if (dSqrToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToEnemy;
                nearest = enemy;
            }
        }

        return nearest;
    }
}